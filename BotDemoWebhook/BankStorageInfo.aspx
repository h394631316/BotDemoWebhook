<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BankStorageInfo.aspx.cs" Inherits="BotDemoWebhook.Web.BankStorageInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        @font-face {
            font-family: "chatbot";
            src: url("../fonts/chatbot.eot");
            src: url("../fonts/chatbot.eot?#iefix") format("embedded-opentype"), url("../fonts/chatbot.woff") format("woff"), url("../fonts/chatbot.ttf") format("truetype"), url("../fonts/chatbot.svg#chatbot") format("svg");
            font-weight: normal;
            font-style: normal;
        }

        body {
            margin: 0px;
            padding: 0px; /*font-size: 62.5%;*/
            font-size: 13px;
            font-family: "Lucida Grande", "Lucida Sans Unicode", Arial, sans-serif;
            background: url(../../images/bg_new.png); /*font-size: 12px;     font-family: '宋体';*/
            overflow: visible;
            width: 100%;
            color: #333;
            height: 100%;
            min-width: 1100px;
        }

        th.auto-style1 {
            width: 377px;
            text-align: center
        }

        .auto-style2 {
            width: 175px;
        }

        .auto-style3 {
            width: 275px;
        }

        .divTable, .operation {
            width: 500px;
        }

        .operation {
            margin-top: 20px;
            text-align: center
        }

        .middleRoundContent {
            background: #fff;
            -webkit-border-radius: 5px;
            -moz-border-radius: 5px;
            border-radius: 5px;
            -webkit-box-shadow: 0 1px 1px rgba(127,127,127,0.3);
            -moz-box-shadow: 0 1px 1px rgba(127,127,127,.3);
            box-shadow: 0 1px 1px rgba(127,127,127,0.3);
            margin: 40px;
            padding-top: 40px;
            padding-left: 40px;
            padding-right: 40px;
            padding-bottom: 60px;
            min-height: 500px;
        }

        .divTitle {
            text-align: left;
            font-size: 24px;
            margin: 0 0 20px 0;
        }

        .btn {
            display: inline-block;
            *border-left: 0 none #e6e6e6;
            border-right: 0 none #e6e6e6;
            border-top: 0 none #e6e6e6;
            border-bottom: 0 none #b3b3b3;
            padding: 0 20px;
            margin-bottom: 0;
            *margin-left: .3em;
            font-size: 14px;
            height: 35px;
            line-height: 35px;
            color: #fff;
            text-align: center;
            text-shadow: 0 1px 1px #0F79B2;
            text-decoration: none;
            cursor: pointer;
            -webkit-border-radius: 5px;
            -moz-border-radius: 5px;
            border-radius: 5px;
            zoom: 1;
            outline: none;
        }

            .btn:hover, .btn:focus {
                text-decoration: none;
                /*-webkit-transition: all 0.1s linear;
	-moz-transition: all 0.1s linear;
	-o-transition: all 0.1s linear;
	transition: all 0.1s linear;*/
            }

            .btn.active, .btn:active {
                outline: 0;
            }


        .btn-blue {
            background: #329FD9;
            background: -moz-linear-gradient(top, #3BACE5 0%, #2594CD 100%);
            background: -webkit-linear-gradient(top, #3BACE5 0%, #2594CD 100%);
            /*background: -ms-linear-gradient(top, #00a5a5 0%, #008e8e 100%);
	background: linear-gradient(to bottom, #00a5a5 0%, #008e8e 100%);*/
            border: 1px solid #329FD9;
            margin-right: 16px;
        }

            .btn-blue:hover {
                background: #329FD9;
                border: 1px solid #329FD9;
            }

            .btn-blue:active, .btn-blue.active {
                background-color: #329FD9;
                -webkit-box-shadow: inset 0 2px 1px rgba(82, 82, 82, 0.3);
                -moz-box-shadow: inset 0 2px 1px rgba(82, 82, 82, 0.3);
                box-shadow: inset 0 2px 1px rgba(82, 82, 82, 0.3);
            }


        .btn.disabled {
            cursor: default;
            opacity: 0.6;
            filter: alpha(opacity=60);
            -webkit-box-shadow: none;
            -moz-box-shadow: none;
            box-shadow: none;
            pointer-events: none;
        }

        .btn-blue.disabled {
            background: #329FD9;
        }
        /*---------------table-----------------*/

        /*--------the-table--------*/
        .the-table {
            width: 100%;
            color: #4a4a4a;
            border-collapse: collapse;
            table-layout: fixed;
        }

            .the-table tr:last-child td:first-child {
                -webkit-border-bottom-left-radius: 3px;
                -moz-border-radius-bottomleft: 3px;
                border-bottom-left-radius: 3px;
            }

            .the-table tr:last-child td:last-child {
                -webkit-border-bottom-right-radius: 3px;
                -moz-border-radius-bottomright: 3px;
                border-bottom-right-radius: 3px;
            }

            .the-table .content_table_topleftcorner {
                -webkit-border-top-left-radius: 3px;
                -moz-border-radius-topleft: 3px;
                border-top-left-radius: 3px;
            }

            .the-table .content_table_toprightcorner {
                -webkit-border-top-right-radius: 3px;
                -moz-border-radius-topright: 3px;
                border-top-right-radius: 3px;
            }

            .the-table .content_table_bottomleftcorner {
                -webkit-border-bottom-left-radius: 3px;
                -moz-border-radius-bottomleft: 3px;
                border-bottom-left-radius: 3px;
            }

            .the-table .content_table_bottomrightcorner {
                -webkit-border-bottom-right-radius: 3px;
                -moz-border-radius-bottomright: 3px;
                border-bottom-right-radius: 3px;
            }

            .the-table td ul {
                padding-left: 20px;
                white-space: nowrap;
                overflow: hidden;
                text-overflow: ellipsis;
                -ms-text-overflow: ellipsis;
                margin: 0;
            }

            .the-table th, .the-table td {
                padding: 3px 5px;
            }

            .the-table th {
                text-align: left;
                background: #E1E1E1;
                height: 35px;
                line-height: 35px;
                font-weight: 400;
            }

            .the-table td {
                text-align: left;
                padding-top: 8px;
                padding-bottom: 8px;
                white-space: nowrap;
                overflow: hidden;
                text-overflow: ellipsis;
                -ms-text-overflow: ellipsis;
            }

                .the-table td.tdnormal {
                    white-space: inherit;
                    text-overflow: inherit;
                    -ms-text-overflow: inherit;
                }

                .the-table th.th_center, .the-table td.td_center {
                    text-align: center;
                }

                .the-table td ol {
                    padding-left: 20px;
                    margin-top: 5px;
                    margin-bottom: 5px;
                }

                .the-table td.td_operation a {
                    margin-right: 2px;
                }

            .the-table .td_operation img, .the-table .td_operation input[type=image] {
                opacity: 1;
                filter: alpha(opacity=100);
            }

                .the-table .td_operation img:hover, .the-table .td_operation input[type=image]:hover {
                    opacity: 0.8;
                    filter: alpha(opacity=80);
                    -webkit-transition: opacity 0.2s ease-in;
                    -moz-transition: opacity 0.2s ease-in;
                    transition: opacity 0.2s ease-in;
                }

            .the-table tr th, .the-table tr td {
            }

                .the-table tr th:first-child, .the-table tr td:first-child {
                    border-left: none;
                    padding-left: 15px;
                }

            .the-table .cth {
                text-align: center;
            }

            .the-table .ctd {
                text-align: center;
            }

            .the-table tr {
                -webkit-transition: background 0.2s ease-in;
                -moz-transition: background 0.2s ease-in;
                transition: background 0.2s ease-in;
            }

                .the-table tr:hover td {
                    background: #fff;
                }

                .the-table tr.trHightLightTotal:hover td {
                    background-color: #D7D7D7;
                    font-weight: bold;
                }

                .the-table tr.trHightLight:hover td {
                    background-color: #D7D7D7;
                }

            .the-table th {
                text-align: left;
                color: #333333;
                height: 24px;
            }

            .the-table .trHead td a:link {
                text-decoration: none;
            }

            .the-table .trTotal td {
                font-weight: bold;
            }

            .the-table .trStyle1 td {
                background-color: #EBEBEB;
            }

            .the-table .trStyle2 td {
                background-color: #F5F5F5;
            }

            .the-table .trHightLightTotal {
                background-color: #D7D7D7;
                font-weight: bold;
            }

            .the-table .trHightLight {
                background-color: #D7D7D7;
            }

            .the-table th a:link {
                color: #333333;
                text-decoration: none;
            }

            .the-table th a:visited {
                color: #333333;
                text-decoration: none;
            }

            .the-table th a:hover {
                color: #333333;
                text-decoration: underline !important;
            }

            .the-table ttd {
                text-align: left;
                vertical-align: middle;
            }

            .the-table td.normal_space {
                white-space: normal;
            }

        .fieldsetCp {
            padding-bottom: 20px;
            padding-left: 20px;
        }

        /*--------form table-----------*/
        .form-table {
        }

            .form-table > tbody > tr > td {
                padding-bottom: 8px;
            }

            .form-table td.td_indent {
                padding-left: 23px;
            }

            .form-table td.td_top {
                vertical-align: top;
            }

            .form-table tr {
                margin: 0;
            }

            .form-table .ttd {
                font-weight: 400;
                padding-right: 10px;
            }

            .form-table .ctd {
                text-align: left;
                vertical-align: middle;
                color: #333;
                padding-right: 5px;
            }

            .form-table .rtd {
                text-align: left;
                color: Red;
            }

            .form-table .lttd {
                text-align: left;
                font-weight: bold;
            }

            .form-table .linktd a {
                color: #666;
                text-decoration: none;
            }

                .form-table .linktd a:hover {
                    cursor: pointer;
                    text-decoration: underline;
                }
    </style>
</head>
<body>
    <div class="middleRoundContent" id="middleRoundContent">
        <div class="divTitle">
            Bank Plan Data
        </div>
        <form id="form1" runat="server">
            <div class="divTable">
                <table class="the-table status">
                    <%--<tbody>
                    <tr>
                        <th class="auto-style2"><b>Account</b></th>
                        <th class="auto-style2"><b>Balance</b></th>
                        <th class="auto-style2"><b>Balance</b></th>
                        <th class="auto-style2"><b>Balance</b></th>
                    </tr>
                </tbody>--%>
                    <tr class="status trStyle1 tr-first" data-id="108">
                        <td class="auto-style2">Savings
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtSavings1" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtSavings2" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtSavings3" runat="server" Width="250" />
                        </td>
                    </tr>
                    <tr class="status trStyle2" data-id="109">
                        <td class="auto-style2">Checking
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtChecking1" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtChecking2" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtChecking3" runat="server" Width="250" />
                        </td>
                    </tr>
                    <tr class="status trStyle1" data-id="110">
                        <td class="auto-style2">Address
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtAddress1" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtAddress2" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtAddress3" runat="server" Width="250" />
                        </td>
                    </tr>
                    <tr class="status trStyle2" data-id="111">
                        <td class="auto-style2">Phone
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtPhone1" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtPhone2" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtPhone3" runat="server" Width="250" />
                        </td>
                    </tr>
                    <tr class="status trStyle1" data-id="112">
                        <td class="auto-style2">Security Question #1
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtSecurityQuestion11" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtSecurityQuestion12" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtSecurityQuestion13" runat="server" Width="250" />
                        </td>
                    </tr>
                    <tr class="status trStyle2" data-id="113">
                        <td class="auto-style2">Security Question #2
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtSecurityQuestion21" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtSecurityQuestion22" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtSecurityQuestion23" runat="server" Width="250" />
                        </td>
                    </tr>
                    <tr class="status trStyle1" data-id="114">
                        <td class="auto-style2">Daily Transaction Limit
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtDailyTransactionLimit1" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtDailyTransactionLimit2" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtDailyTransactionLimit3" runat="server" Width="250" />
                        </td>
                    </tr>
                    <tr class="status trStyle2" data-id="115">
                        <td class="auto-style2">Account Number
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtAccountNumber1" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtAccountNumber2" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtAccountNumber3" runat="server" Width="250" />
                        </td>
                    </tr>
                    <tr class="status trStyle1" data-id="116">
                        <td class="auto-style2">Username
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtUsername1" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtUsername2" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtUsername3" runat="server" Width="250" />
                        </td>
                    </tr>
                    <tr class="status trStyle2" data-id="117">
                        <td class="auto-style2">Account Type
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtAccountType1" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtAccountType2" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtAccountType3" runat="server" Width="250" />
                        </td>
                    </tr>
                    <tr class="status trStyle1" data-id="118">
                        <td class="auto-style2">Branch number
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtBranchNumber1" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtBranchNumber2" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtBranchNumber3" runat="server" Width="250" />
                        </td>
                    </tr>
                    <tr class="status trStyle2" data-id="119">
                        <td class="auto-style2">Routing Number
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtRoutingNumber1" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtRoutingNumber2" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtRoutingNumber3" runat="server" Width="250" />
                        </td>
                    </tr>
                    <tr class="status trStyle1" data-id="120">
                        <td class="auto-style2">Transit Number
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtTransitNumber1" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtTransitNumber2" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtTransitNumber3" runat="server" Width="250" />
                        </td>
                    </tr>
                    <tr class="status trStyle2" data-id="121">
                        <td class="auto-style2">Hold Policy
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtHoldPolicy1" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtHoldPolicy2" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtHoldPolicy3" runat="server" Width="250" />
                        </td>
                    </tr>
                    <tr class="status trStyle1" data-id="122">
                        <td class="auto-style2">Overdraft Policy
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtOverdraftPolicy1" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtOverdraftPolicy2" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtOverdraftPolicy3" runat="server" Width="250" />
                        </td>
                    </tr>
                    <tr class="status trStyle2 tr-last" data-id="123">
                        <td class="auto-style2">Password
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtPassword1" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtPassword2" runat="server" Width="250" />
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtPassword3" runat="server" Width="250" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="operation">
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-blue btn-save-changes" Text="Save" OnClick="btnSave_Click" />
            </div>
        </form>
    </div>
</body>
</html>
