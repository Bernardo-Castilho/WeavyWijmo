# ASP.NET Core with React
https://docs.microsoft.com/en-us/aspnet/core/client-side/spa/react?view=aspnetcore-6.0&tabs=visual-studio

# ASP.NET Core with React and Authorization
https://alexcodetuts.com/2020/01/26/asp-net-core-3-1-with-react-user-authentication-and-registration/
dotnet new react -o <output_directory_name> -au Individual

# NPM
npm install bootstrap
npm install @weavy/dropin-js
npm install @grapecity/wijmo.all

#Wijmo/React
import 'bootstrap.css';
import '@grapecity/wijmo.styles/wijmo.css';
import './app.css';
//
import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { FlexGrid, FlexGridColumn } from '@grapecity/wijmo.react.grid';
import { format, SortDescription } from "@grapecity/wijmo";

#Dashboard
* get data
* grid
* chart
- weavy

fetch('https://wijmo.weavy.io/api/auth', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/x-www-form-urlencoded'
    },
    body: new URLSearchParams({
        grant_type: 'client_credentials',
        client_id: 'sandbox',
        client_secret: 'Ux3Ko8vRGjGhfX34ENciHyagqSwbL5EM'
    })
});

let generateJwt = (user) => {
  let KJUR = rsa.KJUR;
  return KJUR.jws.JWS.sign("HS256", 
    JSON.stringify({ alg: "HS256", typ: "JWT" }),
    JSON.stringify({
      iss: "5f2565a0-20c3-41c1-bf3f-f8a87cbac641", // Weavy Client Id
      exp: KJUR.jws.IntDate.get("now + 1day"), // Token expiration date
      sub: user.id, // Unique user id
      email: user.eMail,
      name: user.name,
      username: user.firstName,
      picture: user.imageUrl,
  }),
  "O3H-2Q[o]d/elKPM:eDv-UWutrG:yEGi"); // Weavy Client Secret
}

var claims = {
        iss: "wijmo-weavy-hackathon",
        sub: user.username,
        dir: "hackathon",
        name: user.name,
        picture: user.picture,
        username: user.username
    }

      iss: "5f2565a0-20c3-41c1-bf3f-f8a87cbac641", // Weavy Client Id
      sub: user.id, // Unique user id
      name: user.name,
      picture: user.imageUrl,
      username: user.firstName,
      email: user.eMail,
      exp: KJUR.jws.IntDate.get("now + 1day"), // Token expiration date

