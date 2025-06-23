let btn = document.querySelector('.print');
btn.addEventListener('click', print)
async function print() {
    const products = JSON.stringify(getProducts());
    let form = new FormData();
    form.append('documentNumber', commercialDoc.documentNumber);
    form.append('client', commercialDoc.clientName);
    form.append('products', products);
    let url = `/api/deliveryNote/print`;
    let response = await fetch(url, {
        method: 'POST',
        body: form,
    });
    if (response.ok) {
        let result = await response.text();
        const iframe = document.createElement("iframe");
        iframe.style.position = "absolute";
        iframe.style.top = "-10000px";
        document.body.appendChild(iframe);
        iframe.contentWindow.document.open();
        iframe.contentWindow.document.write(result);
        iframe.contentWindow.document.close();
        iframe.contentWindow.focus();
        iframe.contentWindow.print();
        document.body.removeChild(iframe);
    } else {
        console.log('error');
    }
}