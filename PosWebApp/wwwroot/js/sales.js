// wwwroot/js/sales.js

$(document).ready(function () {
    // Helper function to show toaster notifications
    function showToast(message, type) {
        var backgroundColor;
        if (type === 'success') {
            backgroundColor = "linear-gradient(to right, #00b09b, #96c93d)";
        } else if (type === 'error') {
            backgroundColor = "linear-gradient(to right, #ff5f6d, #ffc371)";
        } else {
            backgroundColor = "linear-gradient(to right, #007bff, #0056b3)";
        }

        Toastify({
            text: message,
            duration: 3000,
            close: true,
            gravity: "top",
            position: "right",
            backgroundColor: backgroundColor,
        }).showToast();
    }

    // Live search functionality
    $('#searchInput').on('input', function () {
        var query = $(this).val();
        var productResults = $('#productResults');
        if (query.length > 2) {
            $.ajax({
                url: searchUrl,
                type: 'GET',
                data: { query: query },
                success: function (items) {
                    productResults.empty();
                    if (items.length > 0) {
                        $.each(items, function (index, item) {
                            var itemName = item.ItemName || item.itemName;
                            var sku = item.SKU || item.sku;
                            var sellingPrice = item.SellingPrice || item.sellingPrice;
                            var quantity = item.Quantity || item.quantity;

                            var cardHtml = `
                                <div class="col">
                                    <div class="card h-100 shadow-sm" style="cursor:pointer;">
                                        <div class="card-body">
                                            <h5 class="card-title">${itemName}</h5>
                                            <h6 class="card-subtitle mb-2 text-muted">${sku}</h6>
                                            <p class="card-text fw-bold">₱${sellingPrice ? sellingPrice.toFixed(2) : '0.00'}</p>
                                            <p class="card-text text-muted small">In Stock: ${quantity}</p>
                                            <button class="btn btn-primary add-item-btn mt-2" data-item-sku="${sku}">Add to Cart</button>
                                        </div>
                                    </div>
                                </div>`;
                            productResults.append(cardHtml);
                        });
                    } else {
                        productResults.append('<div class="col"><p class="text-muted">No products found.</p></div>');
                    }
                }
            });
        } else {
            productResults.empty();
        }
    });

    // Handle Add to Cart button click on product cards
    $(document).on('click', '.add-item-btn', function (e) {
        e.preventDefault();
        var sku = $(this).data('item-sku');
        $.ajax({
            url: addToCartUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ Sku: sku, Quantity: 1 }),
            success: function (response) {
                if (response.success) {
                    updateCartUI(response.cart);
                    $('#searchInput').val('');
                    $('#searchInput').focus();
                    showToast('Item added to cart!', 'success');
                } else {
                    showToast(response.message, 'error');
                }
            },
            error: function (xhr) {
                showToast('An error occurred. Please try again.', 'error');
            }
        });
    });

    // Handle Remove from Cart
    $(document).on('click', '.remove-item', function () {
        var itemId = $(this).data('item-id');
        $.ajax({
            url: removeFromCartUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ ItemId: itemId }),
            success: function (response) {
                if (response.success) {
                    updateCartUI(response.cart);
                    calculateChange();
                    showToast('Item removed from cart.', 'success');
                } else {
                    showToast(response.message, 'error');
                }
            },
            error: function (xhr) {
                showToast('An error occurred. Please try again.', 'error');
            }
        });
    });

    // Handle Update Quantity
    $(document).on('change', '.item-quantity', function () {
        var itemId = $(this).data('item-id');
        var quantity = $(this).val();
        if (quantity > 0) {
            $.ajax({
                url: updateQuantityUrl,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ ItemId: itemId, Quantity: quantity }),
                success: function (response) {
                    if (response.success) {
                        updateCartUI(response.cart);
                        calculateChange();
                        showToast('Quantity updated.', 'success');
                    } else {
                        showToast(response.message, 'error');
                    }
                },
                error: function (xhr) {
                    showToast('An error occurred. Please try again.', 'error');
                }
            });
        }
    });

    // Handle "Complete Transaction" button click
    $('#completeTransactionBtn').on('click', function () {
        var totalDue = parseFloat($('#totalDueDisplay').text().replace('₱', '')) || 0;
        var cashPaid = parseFloat($('#cashPaidInput').val()) || 0;

        if (cashPaid < totalDue) {
            showToast('Cash paid must be greater than or equal to the total due.', 'error');
            return;
        }

        // Use the global function to show the confirmation modal
        showConfirmModal('Are you sure you want to complete this transaction?', function () {
            // This code runs when the user clicks 'Confirm'
            $.ajax({
                url: completeTransactionUrl,
                type: 'POST',
                success: function (response) {
                    if (response.success) {
                        showToast(response.message, 'success');
                        updateCartUI([]);
                        $('#cashPaidInput').val('0.00');
                        calculateChange();
                    } else {
                        showToast(response.message, 'error');
                    }
                },
                error: function (xhr) {
                    showToast('An error occurred while completing the transaction. Please try again.', 'error');
                }
            });
        });
    });

    // Function to update the cart display
    function updateCartUI(cart) {
        var cartTableBody = $('#cartTable tbody');
        cartTableBody.empty();
        var totalAmount = 0;
        if (cart && cart.length > 0) {
            $.each(cart, function (index, item) {
                var price = item.sellingPrice ? item.sellingPrice : 0;
                var subtotal = price * item.quantity;
                var row = `<tr>
                    <td>${item.itemName}</td>
                    <td><input type="number" class="form-control item-quantity" data-item-id="${item.id}" value="${item.quantity}" min="1" style="width: 90px;"></td>
                    <td>₱${price.toFixed(2)}</td>
                    <td>₱${subtotal.toFixed(2)}</td>
                    <td><button class="btn btn-danger btn-sm remove-item" data-item-id="${item.id}">Remove</button></td>
                </tr>`;
                cartTableBody.append(row);
                totalAmount += subtotal;
            });
        }
        $('#totalDueDisplay').text('₱' + totalAmount.toFixed(2));
    }

    // Event listener for cash paid input
    $('#cashPaidInput').on('input', function () {
        calculateChange();
    });

    // Helper function to calculate and display change
    function calculateChange() {
        var totalDue = parseFloat($('#totalDueDisplay').text().replace('₱', '')) || 0;
        var cashPaid = parseFloat($('#cashPaidInput').val()) || 0;
        var change = cashPaid - totalDue;
        $('#changeDisplay').text('₱' + change.toFixed(2));
    }
});