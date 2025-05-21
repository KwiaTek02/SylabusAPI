﻿using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using SylabusAPI.Data;
using SylabusAPI.DTOs;
using SylabusAPI.Models;
using SylabusAPI.Settings;
using SylabusAPI.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace SylabusAPI.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly SyllabusContext _db;
        private readonly JwtSettings _jwt;

        public AuthService(SyllabusContext db, IOptions<JwtSettings> jwtOptions)
        {
            _db = db;
            _jwt = jwtOptions.Value;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            // wygeneruj sól
            var saltBytes = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(saltBytes);
            var salt = Convert.ToBase64String(saltBytes);

            // hash hasła
            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: request.Password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100_000,
                numBytesRequested: 32));

            var user = new uzytkownicy
            {
                imie_nazwisko = request.ImieNazwisko,
                tytul = request.Tytul,
                login = request.Login,
                haslo = hash,
                sol = salt,
                email = request.Email,
                typ_konta = request.TypKonta
            };

            _db.uzytkownicies.Add(user);
            await _db.SaveChangesAsync();

            return GenerateToken(user.id, user.login, user.typ_konta);
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _db.uzytkownicies.FirstOrDefaultAsync(u => u.login == request.Login);
            if (user is null)
                throw new UnauthorizedAccessException("Nieprawidłowy login lub hasło.");

            var saltBytes = Convert.FromBase64String(user.sol!);
            var hashAttempt = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: request.Password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100_000,
                numBytesRequested: 32));

            if (hashAttempt != user.haslo)
                throw new UnauthorizedAccessException("Nieprawidłowy login lub hasło.");

            return GenerateToken(user.id, user.login, user.typ_konta);
        }

        private AuthResponse GenerateToken(int userId, string login, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim("login", login),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_jwt.ExpiryMinutes);

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return new AuthResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = expires
            };
        }
    }
}