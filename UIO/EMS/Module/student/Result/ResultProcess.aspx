<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="ResultProcess.aspx.cs" Inherits="EMS.Module.student.Result.ResultProcess" %>
<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>
<%@ Register Src="~/UserControls/AdmissionSessionUserControl.ascx" TagPrefix="uc1" TagName="AdmissionSessionUserControl" %>
<%@ Register Src="~/UserControls/BatchUserControl.ascx" TagPrefix="uc1" TagName="BatchUserControl" %>
<%@ Register Src="~/UserControls/SessionUserControl.ascx" TagPrefix="uc1" TagName="SessionUserControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Student Result Process
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
     <script src="../../../JavaScript/jquery-1.6.1.min.js"></script>
    <script src="../../../JavaScript/jquery-1.7.1.js"></script>
    <script> 
        function InProgress() {
            var panelProg = $get('divProgress');
            panelProg.style.display = '';
        }

        function onComplete() {
            var panelProg = $get('divProgress');
            panelProg.style.display = 'none';
        }

        function ProcessResult() {
            InProgress();
            var program = $("#ctl00_MainContainer_ucProgram_ddlProgram").val();

           

            var programName = $("#ctl00_MainContainer_ucProgram_ddlProgram option:selected").text();
            var examId = $("#<%= ddlExam.ClientID %>").val();

  
          //  console.log(programName);

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "ResultProcess.aspx/ExecuteResultProcess",
                data: "{'ProgramId':'" + program + "','ExamId':'" + examId+ "'}",
                dataType: "json",

                success: function (data) {

                    onComplete();
                    var parsed = JSON.parse(data.d);
                   
                    if(parsed[0].NoData == 1)
                    {
                        alert('No data found for result process');
                    }
                    else
                    {
                        if (parsed[0].Inserted > 0) {
                            alert('Result process is done successfully');
                        }
                        else if (parsed[0].Updated > 0) {
                            alert('Result has been updated');
                        }
                        else
                        {
                            alert('Result process is done already.');
                        }
                    }
                    // console.log(findDistinctStudent(parsed));
                   
                },
                error: function (e) {
                    console.log(e);
                },
            });

        }
    </script>
      
        
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">
    <div>
         <div class="PageTitle">
        <label>Result Process</label>
    </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table style="width: 100%;" id="tblHeader">
                        <tr>
                            <td colspan="2" class="style1">
                                <table style="width: 100%;">
                                    <tr>
                                        <td class="auto-style4">
                                            <asp:Label  ID="Label4" runat="server" Text="Department : "></asp:Label>
                                        </td>
                                        <td class="auto-style2" style="padding-right:20px;">
                                            <%--<asp:DropDownList ID="programDropDownList" width="250px" AutoPostBack="True" OnSelectedIndexChanged="programDropDownList_OnSelectedIndexChanged" runat="server"></asp:DropDownList>--%>
                                            <uc1:DepartmentUserControl runat="server" ID="ucDepartment" OnDepartmentSelectedIndexChanged="OnDepartmentSelectedIndexChanged" />
                                        </td>
                                        <td class="auto-style4">
                                            <asp:Label  ID="Label3" runat="server" Text="Program : "></asp:Label>
                                        </td>
                                        <td class="auto-style2"  style="width:420px;padding-right:20px;">
                                            <%--<asp:DropDownList ID="programDropDownList" width="250px" AutoPostBack="True" OnSelectedIndexChanged="programDropDownList_OnSelectedIndexChanged" runat="server"></asp:DropDownList>--%>
                                            <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="OnProgramSelectedIndexChanged"   />
                                        </td>

                                         <td class="auto-style4"><asp:Label ID="Label1" runat="server" Text="Session : "></asp:Label></td>
                         <td class="auto-style2">
                                            <%--<asp:DropDownList ID="programDropDownList" width="250px" AutoPostBack="True" OnSelectedIndexChanged="programDropDownList_OnSelectedIndexChanged" runat="server"></asp:DropDownList>--%>
                                            <uc1:AdmissionSessionUserControl runat="server" ID="ucAdmissionSession"   />
                                        </td>
                                        <%--<td><asp:Label ID="Label2" runat="server" Text="Batch"></asp:Label></td>
                                        <td>
                                            <uc1:BatchUserControl runat="server" ID="ucBatch"/>
                                        </td>--%>
                                       
                                
                                    </tr>
                                    <tr></tr>
                                    <tr>
                                        <%--<td class="auto-style4"><asp:Label ID="Label8" runat="server" Text="Final Exam : "></asp:Label></td>
                                        <td class="auto-style2">
                                            <asp:DropDownList ID="ddlFinalExam" Width="150px" runat="server"></asp:DropDownList>
                                        </td>
                                        <td class="auto-style4"><asp:Label ID="Label6" runat="server" Text="Year : "></asp:Label></td>
                                        <td class="auto-style2">
                                            <asp:DropDownList ID="ddlYear" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" runat="server"></asp:DropDownList>
                                        </td>
                                        <td class="auto-style4"><asp:Label ID="Label7" runat="server" Text="Semester : "></asp:Label></td>
                                        <td class="auto-style2">
                                            <asp:DropDownList ID="ddlSemester" Width="150px" runat="server"></asp:DropDownList>
                                        </td>--%>
                                          <tr style="height:50px;">
                        <%--<td class="auto-style4">
                            <asp:Label ID="Label3" Width="120px" runat="server" Text="Versity Session"></asp:Label>
                        </td>
                        <td class="auto-style5">
                            <uc1:AdmissionSessionUserControl Width="150px" runat="server" ID="ucVersitySession" class="margin-zero dropDownList"/>
                        </td>--%>                        
                        <td class="auto-style4"><asp:Label ID="Label8" runat="server" Text="Year : "></asp:Label></td>
                        <td class="auto-style6">
                            <asp:DropDownList ID="ddlYearNo" Width="150px" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlYearNo_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                        <td class="auto-style4"><asp:Label ID="Label9" runat="server" Text="Semester : "></asp:Label></td>
                        <td class="auto-style6" style="width:30px;">
                            <asp:DropDownList ID="ddlSemesterNo" Width="150px"  AutoPostBack="true"  runat="server" OnSelectedIndexChanged="ddlSemesterNo_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                        <td class="auto-style4">
                            <asp:Label ID="Label10" runat="server" Text="Exam : "></asp:Label>
                        </td>
                        <td class="auto-style6">
                            <asp:DropDownList ID="ddlExam" Width="350px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlExam_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                 
                    </tr>    
                                    </tr>
                                    <tr></tr>
                                    <tr>
                                       <td class="auto-style4">
                                           <button onclick="ProcessResult()"> Process Result </button>
<%--                                            <asp:Button ID="btnSaveToServer" runat="server" Text="Upload Student" OnClick="btnSaveToServer_Click" />--%>
                                       </td>
                                   </tr>
                                </table>
                            </td>
                        </tr>
                    </table>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
     <div>
        
         <div id="divProgress" style="display: none; z-index: 1000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
                    <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/Img/Waiting.gif" Height="150px" Width="150px" />
                </div>
    </div>
</asp:Content>
