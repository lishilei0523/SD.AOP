//定义全局变量$tbGrid，用于刷新Grid
var $tbGrid = null;

//DOM初始化事件
$(function () {
    debugger;
    //加载列表
    searchRunningLogs();
});

//加载用户列表
function getRunningLogs(queryParams) {

    //指定url
    $.easyuiExt.datagrid.url = "/RunningLog/GetRunningLogs";

    //定义列
    $.easyuiExt.datagrid.columns = [
        [
            { field: "ck", checkbox: true, halign: "center" },
            { field: "Id", title: "Id", halign: "center", hidden: true },
            { field: "ClassName", title: "类名", halign: "center" },
            { field: "MethodName", title: "方法名", halign: "center" },
            { field: "MethodType", title: "方法类型", halign: "center" },
            { field: "ReturnValueType", title: "返回值类型", halign: "center" },
            { field: "StartTimeString", title: "开始时间", halign: "center" },
            { field: "EndTimeString", title: "结束时间", halign: "center" },
            { field: "IPAddress", title: "IP地址", align: "center", halign: "center" },
            {
                field: "Watch",
                title: "查看",
                width: 35,
                formatter: function (value, row) {
                    var start = "<a class=\"aLink\" href=\"javascript: watchLog('";
                    var end = "');\" >查看</a>";
                    var element = start + row.Id + end;

                    return element;
                }
            },
            {
                field: "Remove",
                title: "删除",
                width: 35,
                formatter: function (value, row) {
                    var start = "<a class=\"aLink\" href=\"javascript: removeLog('";
                    var end = "');\" >删除</a>";
                    var element = start + row.Id + end;

                    return element;
                }
            }
        ]
    ];

    //初始化工具栏
    $.easyuiExt.datagrid.toolbarId = "#toolbar";

    //绑定参数模型
    $.easyuiExt.datagrid.queryParams = queryParams;

    //绑定$tbGrid
    $tbGrid = $("#tbGrid").datagrid($.easyuiExt.datagrid);
}

//搜索
function searchRunningLogs() {
    var form = $("#frmQueries");
    var queryParams = $.global.formatForm(form);

    getRunningLogs(queryParams);
}

//查看日志
function watchLog(loginId) {
    $.easyuiExt.showWindow("查看日志", "/RunningLog/Detail/" + loginId, 1024, 600);
}

//删除日志
function removeLog(loginId) {
    $.easyuiExt.messager.confirm("Warning", "确定要删除吗？", function (confirm) {
        if (confirm) {
            $.ajax({
                type: "post",
                url: "/RunningLog/RemoveLog/" + loginId,
                success: function () {
                    $.easyuiExt.messager.alert("OK", "删除成功！");
                    $.easyuiExt.updateGridInTab();
                },
                error: function (error) {
                    $.easyuiExt.messager.alert("Error", error.responseText);
                }
            });
        }
    });
}

//删除选中日志
function removeLogs() {
    //获取所有的选中行
    var checkedRows = $("#tbGrid").datagrid("getChecked");

    //判断用户有没有选中
    if (checkedRows.length > 0) {
        $.easyuiExt.messager.confirm("Warning", "确定要删除吗？", function (confirm) {
            if (confirm) {
                //填充用户登录名数组
                var checkedLoginIds = [];
                for (var i = 0; i < checkedRows.length; i++) {
                    checkedLoginIds.push(checkedRows[i].Id);
                }

                //JSON格式转换
                var params = $.global.appendArray(null, checkedLoginIds, "logIds");

                $.ajax({
                    type: "POST",
                    url: "/RunningLog/RemoveLogs",
                    data: params,
                    success: function () {
                        $.easyuiExt.messager.alert("OK", "删除成功！");
                        $.easyuiExt.updateGridInTab();
                    },
                    error: function (error) {
                        $.easyuiExt.messager.alert("Error", error.responseText);
                    }
                });
            }
        });
    }
    else {
        $.easyuiExt.messager.alert("Warning", "请选中要删除的日志！");
    }
}