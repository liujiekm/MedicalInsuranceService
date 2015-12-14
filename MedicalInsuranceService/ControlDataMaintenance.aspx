<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ControlDataMaintenance.aspx.cs" Inherits="MedicalInsuranceService.ControlDataMaintenance" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>阳光医保对照数据录入</title>
    <link href="Common/css/bootstrap.css" rel="stylesheet" />
    <link href="Common/css/selectpick.css" rel="stylesheet" />

    <script src="Common/js/jquery-1.10.2.js"></script>
    <script src="Common/js/knockout-3.2.0.js"></script>
    <script src="Common/js/bootstrap.js"></script>
    <script src="Common/js/selectpick.js"></script>
    <script type="text/javascript">
        //web api 地址
        var url = "maintenance/";

        $(function () {
            //绑定ko对象
            ko.applyBindings(ViewModel);
        });

        //ko模型对象
        function ViewModel() {
            var self = this;
            //对照列表
            self.controlList = ko.observableArray();
            //当前操作对照ID
            self.currentControlID = ko.observable(0);
            //当前选中类别
            self.currentType = ko.observable("yllb");
            //当前类别对应的对照列表
            self.currentTypeOfControlList = ko.observableArray();
            //当前查询显示的类别
            self.SearchType = ko.observable("ypsypl");

            $(function () {
                //初始化下拉列表框
                $("#type").selectpick({
                    width: "100px", selectText: "医疗类别", onSelect: function (value, text) {
                        self.currentType(value);
                    }
                });

                $("#SearchType").selectpick({
                    width: "100px", selectText: "药品使用频次", onSelect: function (value, text) {
                        self.SearchType(value);
                        self.GetCurrentTypeOfControlList();
                    }
                });

                self.GetControlList();
            });

            //获取对照列表
            self.GetControlList = function () {
                $.ajax({
                    type: 'get',
                    contentType: 'application/json',
                    url: url + "GetControl",
                    success: function (data) {
                        var json = eval(data);
                        if (json.length >= 1) {
                            for (var i = 0; i < json.length; i++) {
                                self.controlList.push(json[i]);
                                if (json[i].Type == self.SearchType()) {
                                    self.currentTypeOfControlList.push(json[i]);
                                }
                            }
                        }
                    }
                });
            };

            //获取当前类别对应的对照列表
            self.GetCurrentTypeOfControlList = function () {
                self.currentTypeOfControlList.removeAll();
                for (var i = 0; i < self.controlList().length; i++) {
                    if (self.controlList()[i].Type == self.SearchType()) {
                    self.currentTypeOfControlList.push(self.controlList()[i]);
                    }
                }

            }

            //设置当前选中对照对象
            self.SetCurrentControl = function (item) {
                self.currentControlID(item.ControlID);
                self.currentType(item.Type);
                $("#socialSecurityCode").val(item.SocialSecurityCode);
                $("#localCode").val(item.LocalCode);
                $(".selectpick_div_type span").first().text(TransformType(item.Type));
                $("#englishName").val(item.EnglishName);
                $("#chineseName").val(item.ChineseName);
                $("#description").val(item.Description);
            }

            //保存对照内容
            self.InsertControl = function () {
                var control = self.GetCurrentControl();
                var jsonData = JSON.stringify(control);
                $.ajax({
                    type: "post",
                    contentType: "application/json",
                    data: jsonData,
                    url: url + "SaveControl",
                    success: function (data) {
                        var json = eval(data);
                        if (json != 0) {
                            control.Type = self.currentType();
                            //如果新增就在对照列表尾部插入
                            //如果是修改则将对照列表中对应的内容更新掉
                            if (control.ControlID == 0) {
                                control.ControlID = json;
                                self.controlList.unshift(control);
                                //当前添加的类别，如果跟选中的查询类别不一致，则以添加类别为条件显示数据
                                if (self.currentType() == self.SearchType()) {
                                    self.currentTypeOfControlList.unshift(control);
                                } else {
                                    self.SearchType(self.currentType());
                                    $(".selectpick_div_SearchType span").first().text(TransformType(self.currentType()));
                                    self.GetCurrentTypeOfControlList();
                                }
                            } else {
                                for (var i = 0; i < self.controlList().length; i++) {
                                    if (self.controlList()[i].ControlID == control.ControlID) {
                                        //先删除之前的，在重新绑定现在的
                                        self.controlList.splice(i, 1);
                                        self.controlList.splice(i, 0, control);
                                    }
                                }
                                //当前修改的类别，如果跟选中的查询类别不一致，则以修改类别为条件显示数据
                                if (self.SearchType() == self.currentType()) {
                                    for (var i = 0; i < self.currentTypeOfControlList().length; i++) {
                                        if (self.currentTypeOfControlList()[i].ControlID == control.ControlID) {
                                            //先删除之前的，在重新绑定现在的
                                            self.currentTypeOfControlList.splice(i, 1);
                                            self.currentTypeOfControlList.splice(i, 0, control);
                                        }
                                    }
                                } else {
                                    self.SearchType(self.currentType());
                                    self.GetCurrentTypeOfControlList();
                                }
                            }
                            self.Clear();
                        } else {
                            alert("保存失败！");
                        }
                    }
                });
            }

            //根据对照ID删除对照
            self.DeleteControl = function (item) {
                if (confirm("确定要删除该对照？")) {
                    $.ajax({
                        type: "post",
                        contentType: "application/json",
                        url: url + "DeleteControl/" + item.ControlID,
                        success: function (data) {
                            var json = eval(data);
                            if (json != 0) {
                                self.controlList.remove(item);
                            } else {
                                alert("删除失败！");
                            }
                        }
                    });
                }
            }

            //获取当前输入的对照对象
            self.GetCurrentControl = function () {
                var controlID = self.currentControlID();
                var socialSecurityCode = $("#socialSecurityCode").val();
                var localCode = $("#localCode").val();
                var type = self.currentType();
                var englishName = $("#englishName").val();
                var chineseName = $("#chineseName").val();
                var description = $("#description").val();
                var control = new Control(controlID, socialSecurityCode, localCode, type, englishName, chineseName, description);
                return control;
            }

            //清空文本框
            self.Clear = function () {
                //将当前选中对照ID置零
                self.currentControlID(0);
                //将当前类别置空
                //self.currentType("");
                $("#socialSecurityCode").val("");
                $("#localCode").val("");
                //$(".selectpick_div_type span").first().text("请选择");
                $("#englishName").val("");
                $("#chineseName").val("");
                $("#description").val("");
            }

            //转换类别
            self.TransformType = function (type) {
                switch (type) {
                    case "yllb":
                        return "医疗类别";
                    case "gytj":
                        return "给药途径";
                    case "jldw":
                        return "剂量单位";
                    case "jxlb":
                        return "剂型类别";
                    case "ypsypl":
                        return "药品使用频次";
                    default:
                        return "";
                }
            }
        }

        //创建对照对象
        function Control(controlID, socialSecurityCode, localCode, type, englishName, chineseName, description) {
            var control = this;
            control.ControlID = controlID;
            control.SocialSecurityCode = socialSecurityCode;
            control.LocalCode = localCode;
            control.Type = type;
            control.EnglishName = englishName;
            control.ChineseName = chineseName;
            control.Description = description;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="panel panel-default">
                <div class="panel-heading">数据录入区</div>
                <div class="panel-body">
                    <div class="form-group">
                        <label for="socialSecurityCode" class="col-sm-2 control-label">社保代码</label>
                        <div class="col-sm-10">
                            <input type="text" class="form-control" id="socialSecurityCode" placeholder="社保代码" />
                        </div>
                    </div>
                    <br />
                    <br />
                    <div class="form-group">
                        <label for="localCode" class="col-sm-2 control-label">本地代码</label>
                        <div class="col-sm-10">
                            <input type="text" class="form-control" id="localCode" placeholder="本地代码" />
                        </div>
                    </div>
                    <br />
                    <br />
                    <div class="form-group">
                        <label for="type" class="col-sm-2 control-label">对照类别</label>
                        <div class="col-sm-10" style="width: 10px">
                            <select id="type">
                                <option>请选择</option>
                                <option value="yllb">医疗类别</option>
                                <option value="gytj">给药途径</option>
                                <option value="jldw">剂量单位</option>
                                <option value="jxlb">剂型类别</option>
                                <option value="ypsypl">药品使用频次</option>
                            </select>
                        </div>
                    </div>
                    <br />
                    <br />
                    <div class="form-group">
                        <label for="englishName" class="col-sm-2 control-label">英文名字</label>
                        <div class="col-sm-10">
                            <input type="text" class="form-control" id="englishName" placeholder="英文名字" />
                        </div>
                    </div>
                    <br />
                    <br />
                    <div class="form-group">
                        <label for="englishName" class="col-sm-2 control-label">中文名字</label>
                        <div class="col-sm-10">
                            <input type="text" class="form-control" id="chineseName" placeholder="中文名字" />
                        </div>
                    </div>
                    <br />
                    <br />
                    <div class="form-group">
                        <label for="description" class="col-sm-2 control-label">具体描述</label>
                        <div class="col-sm-10">
                            <input type="text" class="form-control" id="description" placeholder="具体描述" />
                        </div>
                    </div>
                    <br />
                    <br />
                    <div class="form-group">
                        <div class="col-sm-offset-2 col-sm-10">
                            <button type="button" class="btn btn-default" data-bind="click: InsertControl">
                                <span class="glyphicon glyphicon-ok" aria-hidden="true"></span>&nbsp;保存
                           
                            </button>
                            <button type="button" class="btn btn-default" data-bind="click: Clear">
                                <span class="glyphicon glyphicon-ok" aria-hidden="true"></span>&nbsp;清空
                           
                            </button>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel panel-default">
                <div class="panel-heading">
                    结果区
                    <label for="SearchType" class="col-sm-2 control-label">结果区</label>                  
                    <label for="SearchType" class="col-sm-1">对照类别</label>
                    <div class="col-sm-10" style="width: 10px">
                        <select id="SearchType">
                            <option>请选择</option>
                            <option value="yllb">医疗类别</option>
                            <option value="gytj">给药途径</option>
                            <option value="jldw">剂量单位</option>
                            <option value="jxlb">剂型类别</option>
                            <option value="ypsypl">药品使用频次</option>
                        </select>
                    </div>
                </div>
                <div class="panel-body">
                    <table class="table table-striped">
                        <thead>
                            <tr>
                                <th>社保代码</th>
                                <th>本地代码</th>
                                <th>对照类型</th>
                                <th>英文名字</th>
                                <th>中文名字</th>
                                <th>具体描述</th>
                                <th>操作</th>
                            </tr>
                        </thead>
                        <tbody data-bind="foreach: currentTypeOfControlList">
                            <tr>
                                <td><span data-bind="text: SocialSecurityCode"></span></td>
                                <td><span data-bind="text: LocalCode"></span></td>
                                <td><span data-bind="text: TransformType(Type)"></span></td>
                                <td><span data-bind="text: EnglishName"></span></td>
                                <td><span data-bind="text: ChineseName"></span></td>
                                <td>
                                    <span data-bind="text: Description"></span>
                                </td>
                                <td>
                                    <button class="btn btn-default" type="button" data-bind="click: SetCurrentControl">
                                        <span class="glyphicon glyphicon-pencil"></span>&nbsp;修改
                                   
                                    </button>
                                    <button class="btn btn-default" type="button" data-bind="click: DeleteControl">
                                        <span class="glyphicon glyphicon-remove"></span>&nbsp;删除
                                   
                                    </button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
