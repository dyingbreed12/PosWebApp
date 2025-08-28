$(document).ready(function () {
    // Handle form submission with AJAX
    $('#addToCartForm').submit(function (e) {
        e.preventDefault();
        var sku = $('#skuInput').val();

        $.ajax({
            url: '@Url.Action("AddToCart", "Sales")',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ Sku: sku, Quantity: 1 }),
            success: function (response) {
                if (response.success) {
                    updateCartUI(response.cart);
                    $('#skuInput').val(''); // Clear the input field
                    $('#skuInput').focus(); // Keep the focus for next scan
                } else {
                    alert(response.message);
                }
            },
            error: function (xhr) {
                alert('An error occurred. Please try again.');
            }
        });
    });

    // Function to update the cart display
    function updateCartUI(cart) {
        var cartTableBody = $('#cartTable tbody');
        cartTableBody.empty();
        var totalAmount = 0;
        $.each(cart, function (index, item) {
            var row = `<tr>
                <td>${item.ItemName}</td>
                <td>${item.Quantity}</td>
                <td>$${item.Price.toFixed(2)}</td>
                <td>$${(item.Price * item.Quantity).toFixed(2)}</td>
            </tr>`;
            cartTableBody.append(row);
            totalAmount += item.Price * item.Quantity;
        });
        $('#totalAmountDisplay').text('$' + totalAmount.toFixed(2));
    }
});