$(document).ready(function () {
    $("#generateReportBtn").click(function () {
        var startDate = $("#startDate").val();
        var endDate = $("#endDate").val();

        $.ajax({
            url: "/Reports/GetSalesHistory",
            type: "GET",
            data: { startDate: startDate, endDate: endDate },
            success: function (data) {
                var tableBody = $("#salesReportTable tbody");
                tableBody.empty();
                data.forEach(function (item) {
                    var row = `<tr>
                        <td>${new Date(item.transactionDate).toLocaleDateString()}</td>
                        <td>${item.cashier}</td>
                        <td>${item.itemName}</td>
                        <td>${item.quantity}</td>
                        <td>$${item.itemPrice.toFixed(2)}</td>
                        <td>$${item.totalAmount.toFixed(2)}</td>
                    </tr>`;
                    tableBody.append(row);
                });
            },
            error: function () {
                alert("Error fetching report data.");
            }
        });
    });
});