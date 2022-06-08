import React, { Component } from "react";
//import authService from './api-authorization/AuthorizeService'

import "@weavy/dropin-js/dist/weavy-dropin.css";
import Weavy from "@weavy/dropin-js";

import "./Chat.css";

export class Chat extends Component {
    static displayName = Chat.name;

    constructor(props) {
        super(props);

        // Weavy instance and apps
        const weavy = new Weavy({
            url: "https://wijmo.weavy.io",
            jwt: this.getWeavyToken
        });
        weavy.authentication.setJwt(this.getWeavyToken); // needed to support user switching
        weavy.app({
            id: "pureChat",
            type: "messenger",
            container: "#theChat"
        });

        console.log("** Chat created");
    }

    // get Weavy token
    async getWeavyToken() {
        console.log("** Chat logging in as staticUser");
        //var user = await authService.getUser();
        //var response = await fetch(`weavytoken?userId=${user.sub}`);
        var response = await fetch(`weavytoken?userId=staticUser`);
        var token = await response.text();
        return token;
    }

    // render the component
    render() {
        return (
            <div>
                <h1>
                    Weavy Chat
                </h1>
                <p>
                    This is the simplest chat app using Weavy.
                </p>
                <div id="theChat" className="theChat" />
            </div>
        );
    }
}
