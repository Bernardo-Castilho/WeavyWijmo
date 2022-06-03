import React, { Component } from 'react';

export class Home extends Component {
    static displayName = Home.name;

    render() {
        return (
            <div>

                <h1>
                    Wijmo Dashboard with Weavy
                </h1>

                <p>
                    This app shows how you can use
                    {" "}<a href="https://www.weavy.com" target="_blank">Weavy</a>{" "}
                    to add contextual collaboration features such as
                    activity feeds, document sharing, and chats to
                    {" "}<a href="https://www.grapecity.com/wijmo" target="_blank">Wijmo</a>{" "}
                    applications.
                </p>
                <p>
                    Select the <a href="./dashboard">Dashboard</a> link
                    to see Wijmo and Weavy in action.
                </p>
                <p>
                    This is an ASP.NET Core with authentication and a React UI.
                    It was created using the <code>dotnet new react</code> command.
                    For details on the generic parts of the app, please refer to
                    {" "}<a href="https://docs.microsoft.com/en-us/aspnet/core/client-side/spa/react?view=aspnetcore-6.0&tabs=visual-studio">this document</a>.
                </p>
                <p>
                    Wijmo and Weavy components were added using regular npm commands
                    in the <code>ClientApp</code> folder.
                </p>
            </div>
        );
    }
}
