<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="ExamSetupNewVersion.aspx.cs" Inherits="EMS.Module.admin.ExamSetupNewVersion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Exam Held In Relation With Program & Exam Committee Setup
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">

    <!-- jQuery library -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <!-- Bootstrap CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css">

    <!-- Bootstrap JavaScript -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js"></script>


    <script src="//cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>

    <script src="ExamSetupNewVersion.js?v2.0"></script>

    <style>
        select {
            width: 100%;
            height: 70%;
        }

        .select2-container {
            height: 85% !important;
        }

            .select2-container .select2-selection--single {
                height: 70% !important;
            }

        .select2-container--default .select2-selection--single {
            border: 1px solid #ccc !important;
            border-radius: 0px !important;
        }

        .hide {
            display: none;
        }

        #ctl00_MainContainer_ucDepartment_ddlDepartment, #ctl00_MainContainer_ucProgram_ddlProgram, #ctl00_MainContainer_ddlYearNo, #ctl00_MainContainer_ddlSemesterNo, #ctl00_MainContainer_ucSession_ddlSession,
        #ctl00_MainContainer_ucAcademicSession_ddlSession, #ctl00_MainContainer_ddlHeldIn, #ctl00_MainContainer_ddlHeldIn, #ctl00_MainContainer_ddlCalenderDistribution, #ctl00_MainContainer_ddlAddCourseTrimester, #ctl00_MainContainer_ddlCourse {
            height: 40px !important;
            font-size: 20px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">

    <asp:UpdatePanel runat="server" ID="UpdatePanel02">
        <ContentTemplate>
            <div class="row">
                <div class="col-sm-12" style="font-size: 12pt; margin-top: 10pt;">
                    <%--<label><b style="color: black; font-size: 26px">Exam Held In Relation With Program</b></label>--%>
                    <asp:Label ID="lblTitle" runat="server" Text="" Font-Bold="true" Style="color: black; font-size: 26px"></asp:Label>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="container-fluid">


        <div class="row">
            <div class="col-12">
                <div class="card">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-4">
                                <asp:HiddenField ID="RoleField" runat="server" />
                                <b>Choose Department</b>
                                <select name="Department" id="department" onchange="deptChangeFunction(this.value, document.getElementById('<%= RoleField.ClientID %>'))">
                                    <option value="0">Select</option>
                                </select>
                            </div>
                            <div class="col-4">
                                <b>Choose Program</b>
                                <select name="Program" id="program" style="width: 100%" disabled>
                                    <option value="0">Select</option>
                                </select>
                            </div>
                            <div class="col-4">
                                <b>Choose Year</b>
                                <select name="Year" id="year">
                                    <option value="0">Select</option>
                                </select>
                            </div>
                        </div>
                        <br />

                        <div class="row">
                            <div class="col-4">
                                <b>Choose Semester</b>
                                <select name="Semester" id="semester">
                                    <option value="0">Select</option>
                                </select>
                            </div>
                            <div class="col-4">
                                <b>Choose Exam Year</b>
                                <select name="Exam" id="exam" onchange="ExamYearChange(this.value)">
                                    <option value="0">Select</option>
                                </select>
                            </div>
                            <div class="col-4">
                                <b>Choose Session</b>
                                <select name="Session" id="session">
                                    <option value="0">Select</option>
                                </select>
                            </div>
                        </div>
                        <br />


                        <div>

                            <button class="btn btn-primary" id="loadbtn">Click Here To View Info</button>
                            <button id="addnew" type="button" class="btn btn-primary" data-toggle="modal" data-target="#myModal1" style="background-color: #AA66CC; color: white;">
                                Add New Held In Relation With Program
                            </button>
                            <button id="addnewcommittee" type="button" class="btn btn-primary" data-toggle="modal" data-target="#myModal" style="background-color: #AA66CC; color: white; display: none">
                                Click Here To Add/Update Committee Member
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <br />
        <div class="col-14">
            <div class="card">
                <div class="card-body">
                    <table class="table" id="newtable" style="display: none">
                        <thead style="background-color: #0069D9; color: white;">
                            <tr>
                                <th>SL</th>
                                <th>
                                    <input type="checkbox" id="all" name="all" value="All" />
                                    <label for="vehicle1">All</label><br />
                                </th>
                                <th>Program</th>
                                <th>Exam Information</th>
                                <th>Committees</th>
                                <th>Department Information</th>
                                <th>Action</th>
                            </tr>
                        </thead>
                        <tbody id="tablebody">
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <br />

        <!-- Add new committee bulk -->
        <div class="modal" id="myModal" role="dialog">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">

                    <!-- Modal Header -->
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>

                    <!-- Modal body -->
                    <div class="modal-body">
                        <div class="container">

                            <div class="row">
                                <%--<asp:HiddenField ID="hdnRelationId" runat="server" />
                                <asp:HiddenField ID="hdnSetupId" runat="server" />--%>

                                <input type="text" id="hdnRelationID" style="display: none" />
                                <input type="text" id="hdnSetupID" style="display: none" />
                                <input type="text" id="hdnbtn" style="display: none" />
                            </div>

                            <div class="row">
                                <div class="col-6">
                                    Choose Department
                                    <select class="custom-select" name="Department" id="dept1" onchange="dept1function(this.value,0)">
                                        <option value="0">Select</option>
                                    </select>
                                </div>
                                <div class="col-6">
                                    Choose Chairman
                                    <select class="select2" name="Chairman" id="chairman">
                                        <option value="0">Select</option>
                                    </select>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-6">
                                    Choose Department
                                    <select class="custom-select" name="Department" id="dept2" onchange="dept2function(this.value,0)">
                                        <option value="0">Select</option>
                                    </select>
                                </div>
                                <div class="col-6">
                                    Choose Member one
                                    <select class="select2" name="MemberOne" id="mbmone">
                                        <option value="0">Select</option>
                                    </select>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-6">
                                    Choose Department
                                    <select class="custom-select" name="Department" id="dept3" onchange="dept3function(this.value,0)">
                                        <option value="0">Select</option>
                                    </select>
                                </div>
                                <div class="col-6">
                                    Choose Member Two
                                    <select class="select2" name="MemberTwo" id="mbmtwo">
                                        <option value="0">Select</option>
                                    </select>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-6">
                                    Choose Department
                                    <select class="custom-select" name="Department" id="dept4" onchange="dept4function(this.value,0)">
                                        <option value="0">Select</option>
                                    </select>
                                </div>
                                <div class="col-6">
                                    Choose External Member
                                    <select class="select2" name="ExternalMember" id="extmbm">
                                    </select>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Modal footer -->
                    <div class="modal-footer">
                        <button type="button" class="btn btn-success" id="savemodal">Save</button>
                        <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                    </div>

                </div>
            </div>
        </div>

        <!-- Add new-->
        <div class="modal" id="myModal1" role="dialog">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">

                    <!-- Modal Header -->
                    <div class="modal-header">
                        <div class="row col-lg-12 col-md-12 col-sm-12">
                            <div class="col-lg-9 col-md-9 col-sm-9">
                                <asp:Label ID="Label1" runat="server" Text="Exam Held In Relation With Program" ForeColor="Blue"></asp:Label>
                            </div>
                            <div class="col-lg-3 col-md-3 col-sm-3" style="text-align: right">
                                <button type="button" class="close" data-dismiss="modal" style="color: red; font-size: 25px">&times;</button>
                            </div>
                        </div>
                    </div>

                    <!-- Modal body -->
                    <div class="modal-body">
                        <div class="container">
                            <div class="row">
                                <input type="text" id="hdnID" style="display: none" />
                            </div>
                            <div class="row">
                                <div class="col-6">
                                    <b>Choose Department</b>
                                    <select name="Department" id="department5" onchange="deptPopChangeFunction(this.value,0)">
                                        <option value="0">Select</option>
                                    </select>
                                </div>
                                <div class="col-6">
                                    <b>Choose Program</b>
                                    <select name="Select" id="prog">
                                        <option value="0">Select</option>
                                    </select>
                                </div>
                            </div>
                            <br />
                            <div class="row" style="margin-top: 10px">
                                <div class="col-6">
                                    <b>Choose Year</b>
                                    <select name="Year" id="Year">
                                        <option value="0">Select</option>
                                    </select>
                                </div>
                                <div class="col-6">
                                    <b>Choose Semester</b>
                                    <select name="Select" id="sem">
                                        <option value="0">Select</option>
                                    </select>
                                </div>
                            </div>
                            <br />
                            <div class="row" style="margin-top: 10px">
                                <div class="col-6">
                                    <b>Choose Exam Year</b>
                                    <select name="Department" id="examyear" onchange="changeFunction(0)">
                                        <option value="0">Select</option>
                                    </select>
                                </div>
                                <div class="col-6">
                                    <b>Choose Session</b>
                                    <select name="Select" id="session1" onchange="changeFunction(0)">
                                        <option value="0">Select</option>
                                    </select>
                                </div>
                            </div>
                            <br />
                            <div class="row" style="margin-top: 10px">
                                <div class="col-6">
                                    <b>Choose Held In</b>
                                    <select name="Department" id="examname">
                                        <option value="0">Select</option>
                                    </select>
                                </div>
                                <div class="col-6" style="margin-top: 25px">
                                    <b>Is Active</b>
                                    <input type="checkbox" id="isactive" />

                                </div>
                            </div>
                            <br />
                            <%--<div class="row">
                                <div class="col-6">
                                    Exam Start date
                                    <input type="date" id="startdate" style="width: 100%; height: 70%" />
                                </div>
                                <div class="col-6">
                                    Exam End date
                                    <input type="date" id="enddate" style="width: 100%; height: 70%" />
                                </div>
                            </div>--%>
                            <br />
                        </div>
                    </div>

                    <!-- Modal footer -->
                    <div class="modal-footer">
                        <button type="button" class="btn btn-success" id="savemodal1">Save</button>
                        <button type="button" id="closebtn" class="btn btn-danger" data-dismiss="modal">Close</button>
                    </div>

                </div>
            </div>
        </div>


        <!--Delete modal-->
        <%--        <div class="modal" id="deleteModal">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">

                    <!-- Modal Header -->
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>

                    <!-- Modal body -->
                    <div class="modal-body">
                        <div class="container">
                            <div class="row" style="display: flex; justify-content:center" >
                                <h2>Are You Sure?</h2>                                
                            </div><br />
                            <div class="row" style="display: flex; justify-content:center">
                                <button type="button" class="btn btn-danger" style="padding:10px; margin:10px">Delete</button>
                                <button type="button" class="btn btn-secondary" data-dismiss="modal" style="padding:10px; margin:10px">Cancel</button>
                            </div>
                        </div>
                    </div>

                    <!-- Modal footer -->
                    <div class="modal-footer">
                        <button type="button" id="closebtndel" class="btn btn-info" data-dismiss="modal">Close</button>
                    </div>

                </div>
            </div>
        </div>--%>
    </div>

</asp:Content>
