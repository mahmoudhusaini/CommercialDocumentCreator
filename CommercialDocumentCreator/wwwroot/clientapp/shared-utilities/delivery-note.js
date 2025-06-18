let btn = document.querySelector('.print');
btn.addEventListener('click', print)
async function print() {
    const products = JSON.stringify(getProducts());
    let form = new FormData();
    form.append('documentNumber', commercialDoc.documentNumber);
    form.append('client', commercialDoc.clientName);
    form.append('products', products);
    console.log(products);
    let url = `/api/deliveryNote/print`;
    let response = await fetch(url, {
        method: 'POST',
        body: form,
    });
    if (response.ok) {
        let result = await response.json();
        console.log(result);
    } else {
        console.log('error');
    }
}