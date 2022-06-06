using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using WeavyWijmo.Data;
using WeavyWijmo.Models;

namespace WeavyWijmo.Controllers;

[ApiController]
[Route("[controller]")]
public class WeavyTokenController : ControllerBase
{
    ApplicationDbContext _ctx;
    public WeavyTokenController(ApplicationDbContext ctx)
    {
        _ctx = ctx;
    }

    [HttpGet]
    public string Get(string userId)
    {
        //var strTokenOAuth = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjM4ZjM4ODM0NjhmYzY1OWFiYjQ0NzVmMzYzMTNkMjI1ODVjMmQ3Y2EiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJhY2NvdW50cy5nb29nbGUuY29tIiwiYXpwIjoiMTI2NzEzMTU4ODItcjhvdGVkcDRoOTBidjVvc2k2dmk3b2JtMzN0dWxwajkuYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJhdWQiOiIxMjY3MTMxNTg4Mi1yOG90ZWRwNGg5MGJ2NW9zaTZ2aTdvYm0zM3R1bHBqOS5hcHBzLmdvb2dsZXVzZXJjb250ZW50LmNvbSIsInN1YiI6IjExNzM2OTg4MTE3Njk0NTIwNzY0OSIsImVtYWlsIjoicmFjYXN0aWxob0BnbWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwiYXRfaGFzaCI6InFhUkpZdTdEMjlWVnVvWVNvbmpzV0EiLCJuYW1lIjoiUm9kcmlnbyBDYXN0aWxobyIsInBpY3R1cmUiOiJodHRwczovL2xoMy5nb29nbGV1c2VyY29udGVudC5jb20vYS0vQU9oMTRHalJEeWwzN3hUZGMzV0E3SDlHX2FFLVp2Ni10M0d5d2dzR09JQ09Wdjg9czk2LWMiLCJnaXZlbl9uYW1lIjoiUm9kcmlnbyIsImZhbWlseV9uYW1lIjoiQ2FzdGlsaG8iLCJsb2NhbGUiOiJlbiIsImlhdCI6MTY1NDI3NTAyNiwiZXhwIjoxNjU0Mjc4NjI2LCJqdGkiOiJjYjZhOTFhMTljOWMwZWU1MTg3NjQ1ZTUwYWNlYzcyOGQxNGEyYjliIn0.eZSXZz01VH8wgy80y00-S5JTREot18ajljGc24I7TVKNDo-3xtdHXqwWR0FJUh0cr6I4zObdkxu7ugT6xWzsojM2Bvb8GSGfUzGBQREVq4mZCDreSLSO-Li7gylpUQ_ztBQyh9wT6CyMCXK9qh1C0yeaO6wAC_Ix9ttf0m3jWUO-ZUDLy-zDMQgoUSidOkKLHvXdXJTV1CAMLJVVsI_wya5VM2PyF5bvoGRMubae4RxGrSYo6lFUGssmb-pjjGOwfYzFm9MVwIGLQjbOEf4yXh8dUnF_649EL2oj3oSULDpzssJ-GENSKzHT04MQ6qHQXtDuyLNDvZb4bdHj2LCl_g";
        var strTokenSite = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJnZXQud2VhdnkuaW8iLCJzdWIiOjEyNjksImRpciI6IlNhbmRib3giLCJnaXZlbl9uYW1lIjoiQmVybmFyZG8iLCJmYW1pbHlfbmFtZSI6ImRlIENhc3RpbGhvIiwiZW1haWwiOiJiZXJuYXJkby1jYXN0aWxob0Bob3RtYWlsLmNvbSIsImV4cCI6MTY1NDgwMzE0NH0.5QPHNR6WdbH4q38YQYm7eQEgYtGdxcPlV-dIn8jODtw";
        //var handler = new JwtSecurityTokenHandler();
        //var ttt = handler.ReadJwtToken(strTokenSite);

        if (userId == "fakeAuth")
        {
            return strTokenSite;
        }
        var user = _ctx.Users.Find(userId);
        if (user != null)
        {
            return strTokenSite;
            //return GenerateToken(user);
        }
        return "";
    }

    // generate a WJT token to use with Weavy
    private static string GenerateToken(ApplicationUser user)
    {
        // get first and last names from the email
        ParseEmail(user.Email, out string firstName, out string lastName);

        //var secret = "dvG7gDigjzamH3g3DhwnyWFJbSZCSJC2"; // get.weavy.io
        var secret = "Ux3Ko8vRGjGhfX34ENciHyagqSwbL5EM"; // sandbox
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim("iss", "sandbox"),//"get.weavy.io"), // Weavy Client Id
                new Claim("dir", "Sandbox"), // what's this?
                new Claim("sub", user.Id), // unique user id
                //new Claim("sub", "1269", ClaimValueTypes.Integer64),
                new Claim("given_name", firstName),
                new Claim("family_name", lastName),
                new Claim("email", user.Email),
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
        };
        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(descriptor);
        return handler.WriteToken(token);
    }
    private static void ParseEmail(string eMail, out string firstName, out string lastName)
    {
        firstName = lastName = eMail;
        eMail = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(eMail);
        var rx = new Regex(@"([A-Za-z]+|@)\s*");
        var matches = rx.Matches(eMail);
        if (matches != null && matches.Count > 0)
        {
            foreach (Match m in matches)
            {
                if (m.Index == 0)
                {
                    firstName = m.Value;
                }
                if (m.Value == "@")
                {
                    break;
                }
                lastName = m.Value;
            }
        }
    }

    private static string GenerateToken(string fakeToken)
    {
        // get data from token
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(fakeToken);
        var dct = jwtSecurityToken.Payload;

        // create a new token with the same data
        var secret = "dvG7gDigjzamH3g3DhwnyWFJbSZCSJC2"; // get.weavy.io
        //var secret = "Ux3Ko8vRGjGhfX34ENciHyagqSwbL5EM"; // sandbox
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim("iss", (string)dct["iss"]), // Weavy Client Id
                new Claim("dir", (string)dct["dir"]), // what's this?
                new Claim("sub", ((Int64)dct["sub"]).ToString(), ClaimValueTypes.Integer64), // unique user id
                new Claim("given_name", (string)dct["given_name"]),
                new Claim("family_name", (string)dct["family_name"]),
                new Claim("email", (string)dct["email"]),
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
        };
        var token = handler.CreateToken(descriptor);
        return handler.WriteToken(token);
    }

}
