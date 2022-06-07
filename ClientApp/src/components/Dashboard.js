import React, { Component } from "react";
import authService from './api-authorization/AuthorizeService'

import "@grapecity/wijmo.styles/wijmo.css";
import { GoogleSheet } from "@grapecity/wijmo.cloud";
import { FlexGrid, FlexGridColumn } from "@grapecity/wijmo.react.grid";
import { FlexGridFilter } from "@grapecity/wijmo.react.grid.filter";
import { FilterType } from "@grapecity/wijmo.grid.filter";
import { FlexChart, FlexChartSeries } from "@grapecity/wijmo.react.chart";
import { Popup } from "@grapecity/wijmo.react.input";

import "@weavy/dropin-js/dist/weavy-dropin.css";
import Weavy from "@weavy/dropin-js";

import "./Dashboard.css";

export class Dashboard extends Component {
    static displayName = Dashboard.name;

    constructor(props) {
        super(props);

        // Google sheet with Sales data
        const SHEET_ID = "1wGuU-8gMIcMHjNBsP99XJ1Ziab5SMwdzS9_mcfSUuT0";
        const API_KEY = "AIzaSyCdupkmi6onZ1f20iYrPY0CJq3fJreGRoU";
        const sheets = new GoogleSheet(SHEET_ID, API_KEY, {
            sheets: ["Sales"],
        });

        // Weavy instance and apps
        const weavy = new Weavy({
            url: "https://wijmo.weavy.io", // sandbox created at https://get.weavy.io/
            //stylesheet: "weavy-dropin.css",
            jwt: this.getWeavyToken
        });
        const messenger = weavy.app({
            id: "messenger",
            type: "messenger",
            container: "#theChat",
            open: false
        });

        // state contains the dashboard data and Weavy messenger app
        this.state = {
            data: sheets.getSheet("Sales"), // get the sales data from the Google sheet
            messenger: messenger, // Weavy messenger app
        };
    }

    // get Weavy token
    async getWeavyToken() {
        var user = await authService.getUser();
        var response = await fetch(`weavytoken?userId=${user.sub}`);
        var token = await response.text();
        return token;
    }

    // init filter to show condition filter for Sales column, no sort buttons
    initFilter(filter) {
        const cf = filter.getColumnFilter("Sales");
        cf.filterType = FilterType.Condition;
        filter.showSortButtons = false;
    }

    // render the dashboard
    render() {
        return (
            <div>
                <h1>
                    Dashboard
                </h1>
                <p>
                    This simple dashboard shows sales data stored in a Google Sheet.
                </p>
                <p>
                    Users may use the grid to sort and filter the data, and 
                    the chart is updated automatically.
                </p>
                <p>
                    Users may also use the chat button below to add comments and
                    check what other users have to say about the data.
                </p>
                <p>
                    This is the chat button: &nbsp;
                    <button
                        id="btnChat"
                        className="btn-weavy"
                        title="Chat">
                        &#x1f4ac;
                    </button>
                </p>
                <Popup id="theChat" className="popup-weavy" owner="#btnChat" shown={
                    () => {
                        requestAnimationFrame(() => {
                            var messenger = this.state.messenger;
                            messenger.reset(); // reconnect iframe after DOM modifications
                            messenger.open();
                        });
                    }
                } />

                <div className="dashboard">

                    {/* sortable, filterable, non-editable grid */}
                    <FlexGrid
                        isReadOnly={true}
                        selectionMode="ListBox"
                        autoGenerateColumns={false}
                        alternatingRowStep={0}
                        headersVisibility="Column"
                        itemsSource={this.state.data}>
                        <FlexGridColumn binding="Product" width="2*" />
                        <FlexGridColumn binding="Sales" format="n0" width="*" />
                        <FlexGridFilter initialized={this.initFilter.bind(this)} />
                    </FlexGrid>

                    {/* synchronized chart */}
                    <FlexChart
                        tooltip={{ content: "<b>{Product}</b><br />{Sales:c2}" }}
                        header="Sales"
                        axisX={{ position: "None" }}
                        axisY={{ position: "None" }}
                        legend={{ position: "None" }}
                        selectionMode="Point"
                        itemsSource={this.state.data}>
                        <FlexChartSeries name="Sales" binding="Sales" />
                    </FlexChart>
                </div>
            </div>
        );
    }
}
