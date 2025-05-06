document.addEventListener("DOMContentLoaded", function() {
    loadProducts();
});

async function loadProducts(brandId = '') {
    const url = brandId ? `/Home/GetProducts?brandId=${brandId}` : '/Home/GetProducts';
    const response = await fetch(url);
    const html = await response.text();
    document.getElementById("productList").innerHTML = html;
}

function filterProducts() {
    const brandId = document.getElementById("brandFilter").value;
    loadProducts(brandId);
}
