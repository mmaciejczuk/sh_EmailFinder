<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TutorialSignalR.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link href="Content/bootstrap.css" rel="stylesheet" />
    <style>
        #myProgress {
            position: relative;
            width: 350px;
            height: 20px;
            background-color: #ddd;
        }

        #myBar {
            position: absolute;
            width: 0%;
            height: 100%;
            background-color: #4CAF50;
        }

        #label {
            text-align: center;
            line-height: 20px;
            color: white;
        }



        #myProgress2 {
            position: relative;
            width: 350px;
            height: 20px;
            background-color: #ddd;
        }

        #myBar2 {
            position: absolute;
            width: 0%;
            height: 100%;
            background-color: #4CAF50;
        }

        #label2 {
            text-align: center;
            line-height: 20px;
            color: white;
        }
    </style>

</head>
<body>
    <div id="MainForm">


        <form runat="server">

            <h3>Email generator</h3>

            <div style="width: 500px">
                <asp:Label ID="LblName" runat="server" Text="Name"></asp:Label>
                <div class="Tb">
                    <asp:TextBox ID="TbName" runat="server" Width="200px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TbName" ErrorMessage="Required Field" ForeColor="Red"></asp:RequiredFieldValidator>
                </div>
                <asp:Label ID="LblSurname" runat="server" Text="Surname"></asp:Label>
                <div class="Tb">
                    <asp:TextBox ID="TbSurname" runat="server" Width="200px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TbSurname" ErrorMessage="Required Field" ForeColor="Red"></asp:RequiredFieldValidator>
                </div>
                <asp:Label ID="LblDomain" runat="server" Text="Domain"></asp:Label>
                <div class="Tb">
                    <asp:TextBox ID="TbDomain" runat="server" Width="200px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TbDomain" ErrorMessage="Required Field" ForeColor="Red"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div style="margin-top: 20px; margin-bottom: 20px;">
                <asp:Button ID="BtnGenerate" runat="server" Text="Generate" OnClick="BtnGenerate_Click" OnClientClick="showProgress()" />
            </div>
            <div>
                <asp:Label ID="LabelError" runat="server" ForeColor="Red"></asp:Label>
                <br />
                <h4>List of avalible emails addresses:</h4>
            </div>
            <div id="main">
                <div id="left" style="float: left;">
                    <asp:CheckBox ID="CheckBoxVE" runat="server" Checked="True" />
                    <asp:Label ID="Label2" runat="server" Text="http://verify-email.org" ForeColor="#000099"></asp:Label>
                    <br />
                    <span id="span"></span>
                    <br />

                    <div>
                        <div>
                            <div id="myProgress">
                                <div id="myBar">
                                    <div id="label">0%</div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <table id="myTable1">
                        <tbody>
                        </tbody>
                    </table>
                    <br />


                    <asp:GridView runat="server" ID="GVVerifyEmail" AutoGenerateColumns="False"
                        BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black"
                        GridLines="Vertical" EmptyDataText="No emails verified. Limit reached.">
                        <Columns>
                            <asp:BoundField DataField="Key" HeaderText="Email" />
                            <asp:BoundField DataField="Value" HeaderText="Is Valid" />
                        </Columns>
                        <FooterStyle BackColor="#CCCCCC" />
                        <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#808080" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#383838" />
                    </asp:GridView>
                    <br />
                    <asp:Label ID="LabelLimitStatus" runat="server"></asp:Label>
                    <br />
                    <asp:Label ID="LabelLimitDesc" runat="server"></asp:Label>
                </div>

                <div id="centre" style="float: left; margin-left: 50px;">
                    <asp:CheckBox ID="CheckBoxMB" runat="server" Checked="True" />
                    <asp:Label ID="Label1" runat="server" Text="https://mailboxlayer.com" ForeColor="#000099"></asp:Label>
                    <br />
                    <span id="span2"></span>
                    <br />

                    <div>
                        <div>
                            <div id="myProgress2">
                                <div id="myBar2">
                                    <div id="label2">0%</div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <table id="myTable2">
                        <tbody>
                        </tbody>
                    </table>
                    <br />

                    <asp:GridView runat="server" ID="GVMailBoxer" AutoGenerateColumns="False"
                        BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black"
                        GridLines="Vertical" EmptyDataText="No emails verified. Limit reached." OnRowDataBound="GVVerifyEmail_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="email" HeaderText="Email" />
                            <asp:BoundField DataField="isValid" HeaderText="Is Valid" />
                        </Columns>
                        <FooterStyle BackColor="#CCCCCC" />
                        <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#808080" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#383838" />
                    </asp:GridView>
                </div>
            </div>
            <br />


        </form>


    </div>


    <script src="scripts/jquery-1.6.4.js"></script>
    <script src="scripts/jquery.signalR-2.2.1.js"></script>

    <script type="text/javascript">

        function showProgress() {
            //var chkStatus1 = document.getElementById("CheckBoxVE");
            //var chkStatus2 = document.getElementById("CheckBoxMB");
            //var div1 = document.getElementById('myProgress');
            //var div2 = document.getElementById('myProgress2');

            //if (chkStatus1.checked) {
                //div1.style.display = 'block';
            //}
            //if (chkStatus2.checked) {
                //div2.style.display = 'block';
            //}
            //else {
            //    div1.style.display = 'none';
            //    div2.style.display = 'none';
            //}

        }

        $(function () {
            var con = $.hubConnection();
            var hub = con.createHubProxy('hitCounter');
            hub.on('onHitRecorded1', function (i, p, q, r) {
                if (i <= 15) {
                    if (p > 100)
                        p = 100;

                    var elem = document.getElementById("myBar");
                    elem.style.width = p + '%';
                    document.getElementById("label").innerHTML = p * 1 + '%';

                    //var x = '<tr><td>' + q + '</td><td>' + r + '</td></tr>';
                    //$('#myTable1').append(x);

                    $('#span').html(i + '&nbsp;of 15');
                }
                else
                    return 0;


            });
            con.start(function () {
                hub.invoke('recordHit');
            });
        })



        $(function () {
            var con = $.hubConnection();
            var hub = con.createHubProxy('hitCounter');
            hub.on('onHitRecorded2', function (i, p, q, r) {
                if (i <= 15) {
                    if (p > 100)
                        p = 100;

                    var elem = document.getElementById("myBar2");
                    elem.style.width = p + '%';
                    document.getElementById("label2").innerHTML = p * 1 + '%';


                    //var x = '<tr><td>' + q + '</td><td>' + r + '</td></tr>';
                    //$('#myTable2').append(x);

                    $('#span2').html(i + '&nbsp;of 15');
                }
                else
                    return 0;

            });
            con.start(function () {
                hub.invoke('recordHit');
            });
        })
    </script>
</body>
</html>
