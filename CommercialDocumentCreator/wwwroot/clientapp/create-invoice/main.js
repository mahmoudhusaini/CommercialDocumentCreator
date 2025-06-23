let addProductBtn = document.querySelector('.add-product-btn');
const tableBody = document.querySelector('.table-body');

const alertBox = document.querySelector('#customAlert');
const alertMessage = document.querySelector('#alertMessage');
let okButton = document.querySelector('#alertOkButton');

const cancelButton = document.querySelector('#alertCancelButton');
cancelButton.onclick = () => {
    alertBox.style.display = 'none';
};

const rateInput = document.querySelector('.rateInput');
const warrantyInput = document.querySelector('.warranty input');
const deliveryDelayInput = document.querySelector('#delivery-delay-input');
const clientNameInput = document.querySelector('.clientNameInput');
const cashDepositInput = document.querySelector('#deposit-amount');

// #region Append Table Row

addProductBtn.addEventListener('click', (event) => {
    let tableRow = document.createElement('tr');

    tableBody.appendChild(tableRow);

    // #region Add Product Name to Table

    function addProductNameTableData() {
        let td = document.createElement('td');
        tableRow.appendChild(td);

        let inputProdName = document.createElement('input');
        Object.assign(inputProdName, {
            type: 'text',
            name: 'name',
            id: 'product-name',
        });
        inputProdName.setAttribute('placeholder', 'Enter product name');
        td.appendChild(inputProdName);
    }

    addProductNameTableData();
    // #endregion

    // #region Add Product Description to Table

    function addProductDescTableData() {
        let td = document.createElement('td');
        tableRow.appendChild(td);

        let inputProdDescription = document.createElement('textarea');
        Object.assign(inputProdDescription, {
            name: 'product-description',
            id: 'product-description',
            required: true,
        });
        inputProdDescription.setAttribute('placeholder', 'Enter product description');
        td.appendChild(inputProdDescription);
    }

    addProductDescTableData();
    // #endregion

    // #region Add Product Quantity to Table

    function addProductQuantityTableData() {
        let td = document.createElement('td');
        tableRow.appendChild(td);

        let inputProdQty = document.createElement('input');
        Object.assign(inputProdQty, {
            type: 'number',
            name: 'quantityInStock',
            id: 'product-quantity',
            value: 1,
            min: 1,
        });
        td.appendChild(inputProdQty);
    }

    addProductQuantityTableData();
    // #endregion

    // #region Add Product Unit Price to Table
    function addProductUnitPriceToTableData() {
        let td = document.createElement('td');
        tableRow.appendChild(td);

        let inputProdUnitPrice = document.createElement('input');
        Object.assign(inputProdUnitPrice, {
            type: 'number',
            name: 'sellingPrice',
            id: 'product-unit-price',
            value: 1,
            required: true,
        });
        td.appendChild(inputProdUnitPrice);
    }

    addProductUnitPriceToTableData();
    // #endregion

    // #region Add Product Total Price to Table
    function addProductTotalPriceToTableData() {
        let td = document.createElement('td');
        tableRow.appendChild(td);

        let inputProdTotalPrice = document.createElement('input');
        Object.assign(inputProdTotalPrice, {
            type: 'number',
            name: 'productTotalPrice',
            id: 'product-total-price',
            value: 1,
            required: true,
        });
        inputProdTotalPrice.setAttribute('readonly', true);
        td.appendChild(inputProdTotalPrice);
    }

    addProductTotalPriceToTableData();
    // #endregion

    // #region Add Product Delete Button to Table

    function addDeleteButton() {
        let td = document.createElement('td');
        tableRow.appendChild(td);

        let deleteButton = document.createElement('button');
        Object.assign(deleteButton, {
            type: 'button',
            name: 'delete-btn',
            id: 'delete-btn',
        });
        const textNode = document.createTextNode("Delete");
        deleteButton.appendChild(textNode);
        td.appendChild(deleteButton);
        deleteButton.className = 'delete-btn';
    }
    addDeleteButton();
    //#endregion

    // #region Add Product Reset Button to Table
    function addResetButton() {
        let td = document.createElement('td');
        tableRow.appendChild(td);

        let resetButton = document.createElement('button');
        Object.assign(resetButton, {
            type: 'button',
            name: 'reset-btn',
            id: 'reset-btn',
        });
        const textNode = document.createTextNode("Reset");
        resetButton.appendChild(textNode);
        td.appendChild(resetButton);
        resetButton.className = 'reset-btn';
    }
    addResetButton();
    //#endregion

});
//#endregion


// #region Reset & Delete Buttons

tableBody.addEventListener('click', (event) => {
    let target = event.target;
    let currentTr = target.closest('tr');

    if (target.tagName === 'BUTTON') {

        switch (target.id) {
            case 'reset-btn':
                let inputs = currentTr.querySelectorAll('input');
                let textarea = currentTr.querySelector('textarea');

                inputs.forEach((input) => {
                    input.value = 0;
                    textarea.value = '';
                });
                break;
            case 'delete-btn':
                if (currentTr && currentTr.parentNode) {
                    currentTr.parentNode.removeChild(currentTr);
                }
                break;
        }
    }
});
// #endregion

// #region Commercial Document Object
let commercialDoc = {
    id: 0,
    invoiceNumber: '',
    creationDate: '',
    clientName: '',
    type: '',
    validityDate: '',
    rate: '',
    isPayable: false,
    warranty: '',
    deliveryDelay: '',
    productsDetails: [],
    totalAmount: 0,
    remainingBalance: 0,
    cashDeposit: 0,
};
//#endregion

// #region Row Total Price Calculation

tableBody.addEventListener('input', (event) => {
    let target = event.target;

    if (target.id === 'product-quantity' || target.id === 'product-unit-price') {

        let currentTr = target.closest('tr');

        const upInput = currentTr.querySelector('#product-unit-price');
        const qtyInput = currentTr.querySelector('#product-quantity')
        const totalPriceInput = currentTr.querySelector('#product-total-price');

        let up = parseFloat(upInput.value);
        let qty = parseInt(qtyInput.value);

        let totalPrice = up * qty;
        totalPriceInput.value = Number(totalPrice);
    }
});
//#endregion



// #region Field Validation Helper Functions

async function inValidateInputField(inputField) {
    inputField.classList.add("invalid-input-field");
    inputField.classList.remove("valid-input-field");
    inputField.setCustomValidity("Invalid Rate Value");
    inputField.reportValidity();
}

async function validateInputField(inputField) {
    inputField.classList.remove("invalid-input-field");
    inputField.classList.add("valid-input-field");
    inputField.setCustomValidity("");
}

function displayAlertMessage(errorMessage) {
    alertMessage.textContent = errorMessage;
    alertBox.style.display = 'flex';

    okButton.style.backgroundColor = "#a93226";

    okButton.onclick = () => {
        alertBox.style.display = 'none';
    };

}

// #endregion



// #region Create Invoice

const createButton = document.querySelector('.create');

createButton.addEventListener('click', (event) => {

    let errorMap = new Map();

    const overAll = calculateOverallTotalAmount() ?? 0;

    let deliveryDelay = deliveryDelayInput.value;
    let warranty = warrantyInput.value;
    const rate = rateInput.value;
    const clientName = clientNameInput.value;
    let cashDepositValue = cashDepositInput.value;

    // #region Validate Fields

    deliveryDelay = Number(deliveryDelay) < 1 ? 1 : deliveryDelay;
    cashDepositValue = Number(cashDepositValue) < 0 ? 0 : cashDepositValue;

    if (!clientName) {
        inValidateInputField(clientNameInput);
        errorMap.set(0, "Invalid Client Name");
    } else {
        validateInputField(clientNameInput);
        errorMap.delete(0);
    }

    if (!rate || rate < 0) {
        inValidateInputField(rateInput);
        errorMap.set(1, "Invalid Rate");
    } else {
        validateInputField(rateInput);
        errorMap.delete(1);
    }

    let prods = getProducts();

    if (prods.length <= 0) {
        const errorMessage = "Invoice Must Have At Least One Product Item";
        displayAlertMessage(errorMessage);

        errorMap.set(2, errorMessage);

    } else {
        errorMap.delete(2);
    }

    if (!warranty) {
        warranty = "Warranty Covers Manufacturer Defacts only";
    }

    if (cashDepositValue > overAll) {
        let errorMessage = "Cash Amount Is Greater Than The Invoice Total";

        displayAlertMessage(errorMessage);

        inValidateInputField(cashDepositInput);
        errorMap.set(3, errorMessage);
    } else {
        validateInputField(cashDepositInput);
        errorMap.delete(3);
    }

    if (errorMap.size > 0) {

        return;
    }

    //#endregion

    const products = JSON.stringify(prods);

    let form = new FormData();

    form.append('id', commercialDoc.id);
    form.append('rate', rate);
    form.append('warranty', warranty);
    form.append('delay', deliveryDelay);
    form.append('client', clientName);
    form.append('cashDeposit', cashDepositValue);
    form.append('overAllAmount', overAll);
    form.append('products', products);

    createInvoice(form);
});


async function createInvoice(form) {
    const url = `/api/Create/Invoice`;
    let response = await fetch(url, {
        method: 'POST',
        body: form,
    });


    if (response.ok) {
        let result = await response.json();

        const documentType = getPaperType(result.type);

        commercialDoc.id = result.id;

        commercialDoc.documentNumber = result.invoiceNumber;
        commercialDoc.creationDate = result.creationDate;
        commercialDoc.clientName = result.clientName;
        commercialDoc.type = documentType;
        commercialDoc.validityDate = result.validityDate;
        commercialDoc.rate = result.rate;
        commercialDoc.isPayable = result.isPayable;
        commercialDoc.warranty = result.warranty;
        commercialDoc.deliveryDelay = result.deliveryDelay;
        commercialDoc.totalAmount = result.totalAmount;
        commercialDoc.remainingBalance = result.remainingBalance;
        commercialDoc.cashDeposit = result.cashDeposit;

        if (commercialDoc.documentNumber) {
            createButton.textContent = 'Update';
        }

        await displayDocumentDetails(commercialDoc);

    } else {
        console.log('response error');
    }
}


//#endregion


// #region Display Document Details

async function displayDocumentDetails(commercialDocObject) {

    let prodsDetails = ``;

    if (commercialDocObject.productsDetails) {

        for (const element of commercialDocObject.productsDetails) {
            prodsDetails += `
                
                <tr>
                    <td><h3>${element.name}</h3></td>
                    <td><h3>${element.sellingPrice}</h3></td>
                    <td><h3>${element.quantityInStock}</h3></td>
                    <td><h3>${element.productTotalPrice}</h3></td>
                    <td><h3>${element.description}</h3></td>
               </tr>
                `
        }
    }


    let details = `
    <div class="container">
        <div class="responsive-table-container">
            <table class="responsive-table">
                <thead>
                  <tr>
                    <th>Product Name</th>
                    <th>Unit Price</th>
                    <th>Quantity</th>
                    <th>Total Price</th>
                    <th>Description</th>
                  </tr>
                </thead>
                <tbody class="table-body">
                        ${prodsDetails}
                </tbody>
            </table>
        </div>
        
        <div class=""><h2>Total Amount: ${commercialDocObject.totalAmount}</h2></div>
        <div class=""><h2>Cash Deposit: ${commercialDocObject.cashDeposit}</h2></div>
        <div class=""><h2>Remaining Balance: ${commercialDocObject.remainingBalance}</h2></div>

        <div class=""><h2>Signature: </h2></div>
        <div class=""><h2>${commercialDocObject.warranty}</h2></div>
        <div class=""><h2>Delivery Delay: ${commercialDocObject.deliveryDelay}</h2></div>

        <hr>
    </div>
  
`
    const iframe = document.createElement("iframe");
    iframe.style.position = "absolute";
    iframe.style.top = "-10000px"; // Hide iframe
    document.body.appendChild(iframe);

    iframe.contentWindow.document.open();
    iframe.contentWindow.document.write(
        `
    <html>
      <head>
        <title>Print</title>
        
        <style>
               body {
                        font-family: 'Cairo', sans-serif;
                    }

               .container {
                            width: 90%;
                            margin: 80px auto;
                        }

                
                .responsive-table-container {
                    width: 100%;
                    overflow-x: auto;
                    margin: 20px auto;
                    border: 1px solid #ddd;
                    border-radius: 10px;
                    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                }

                .responsive-table {
                    width: 100%;
                    border-collapse: collapse;
                    min-width: 600px;
                }

                .responsive-table thead {
                    background-color: #3e97d6;
                    color: #fff;
                }

                .responsive-table th,
                .responsive-table td {
                    text-align: center;
                    padding: 10px;
                    border: 1px solid #ddd;
                }

                .responsive-table th {
                    font-size: 16px;
                    font-weight: bold;
                }

        </style>
        
        <style>
                .nav {
                background-color: #eee;
                width: 100%;
                height: 100px;
                padding: 20px;
                display: grid;
                grid-template-areas: "left-nav left-nav left-nav left-nav left-nav left-nav  right-nav ";
            }
                .left-nav {
                grid-area: left-nav;
                display: flex;
                flex-flow: row nowrap;
                justify-content: start;
                align-items: center;
                font-size: 10px;
            }
                
            .legal-info {
                display: flex;
                flex-flow: column nowrap;
                align-items: start;
            }
            .right-nav {
                display: flex;
                flex-flow: row nowrap;
                justify-content: space-around;
                align-items: center;
            }
                
                
            .content-one {
                margin: 30px;
                font-size: 10px;
                display: flex;
                flex-flow: row nowrap;
                justify-content: space-between;
            }
                
            .content-one-right {
                width: 300px;
                display: grid;
                grid-template-rows: repeat(6, 1fr);
                grid-template-columns: repeat(2, 50%);
                gap: 2px;
            }
                
            .content-one-right input {
                width: 100%;
                padding: 8px 10px;
                border-radius: 5px;
                border: 1px solid #ccc;
                outline: none;
                font-size: 14px;
            }
                
            .content-one-right input:focus {
                border-color: #3e97d6;
                box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
            }
                
            .quotation-nb {
                text-align: center;
                grid-column: span 2;
            }
                
            .content-one-right div {
                border: 1px solid black;
                border-radius: 5px;
                padding: 5px;
            }


        </style>

      </head>
      <body>
            <div class="nav">
                <div class="left-nav">
                    <div class="image-ictech"><img src="/clientapp/images/ictech-2-2.jpg" alt=""></div>
                    <div class="legal-info">
                        <div class="idnat">ID NAT : 01-F4300-N03397F</div>
                        <div class="rccm">RCCM : 22-A-01980</div>
                        <div class="nimpot">N Impot : A2205769W</div>
                    </div>
                </div>
                <div class="right-nav">
                    <div><img src="/clientapp/images/dell.jpg" alt=""></div>
                    <div><img src="/clientapp/images/hp.jpg" alt=""></div>
                    <div><img src="/clientapp/images/lenovo.jpg" alt=""></div>
                    <div><img src="/clientapp/images/apc.png" alt=""></div>
                    <div><img src="/clientapp/images/hikvision.jpg" alt=""></div>
                </div>
            </div>

            <div class="content-one">

                <div class="content-one-left">
                    <h3>Ets ICTech</h3>
                    <h3>South, Saida, Main Street</h3>
                    <h3>Lebanon</h3>
                    <h3>Bank : SOFIBANQUE SA RDC</h3>
                    <h3>Account : 000232011002143080200-17 USD</h3>
                    <h3>SWIFT : SFBXCDKIXXX</h3>
                    <h3>Email: ictech.congo@gmail.com</h3>
                    <h3>Phone Numbre: +961 76095672</h3>
                </div>


                <div class="content-one-right">
                    <div class="quotation-nb">${commercialDocObject.type} : ${commercialDocObject.documentNumber}</div>
                    <div>
                        <h3>Date: </h3>
                    </div>
                    <div>
                        <h3 class="quotation-date">${commercialDocObject.creationDate.slice(0, 10)}</h3>
                    </div>
                    <div>
                        <h3>Client: </h3>
                    </div>
                    <div>
                        <h3>${commercialDocObject.clientName}</h3>
                    </div>
                    <div>
                        <h3>Currency: </h3>
                    </div>
                    <div>
                        <h3>USD</h3>
                    </div>
                    <div>
                        <h3>Valid until: </h3>
                    </div>
                    <div>
                        <h3 class="end-date">${commercialDocObject.validityDate.slice(0, 10)}</h3>
                    </div>
                    <div>
                        <h3>Rate: </h3>
                    </div>
                    <div><h3>${commercialDocObject.rate}</h3></div>

                </div>
            </div>
            <br>
            <hr>

            ${details}
      </body>
    </html>
  `);

    iframe.contentWindow.document.close();

    iframe.contentWindow.focus();
    iframe.contentWindow.print();
    document.body.removeChild(iframe); // Clean up iframe after printing
}
//#endregion




// #region Convert Invoice

const convertSelect = document.querySelector('#options-list');

convertSelect.addEventListener('change', (event) => {

    if (commercialDoc.documentNumber === '') {
        return;
    }

    let target = event.target;
    alertMessage.textContent = "Are You Sure You Want To Convert That Invoice";
    alertBox.style.display = 'flex';

    okButton.style.backgroundColor = "#2e7db4";

    okButton.onclick = () => {
        alertBox.style.display = 'none';
        let type = target.value;
        convertInvoice(type);
    };
});

async function convertInvoice(type) {

    const url = `/api/Convert/Document/${type}`;

    let total = calculateOverallTotalAmount();
    let prods = getProducts();

    const products = JSON.stringify(prods);

    const form = new FormData();

    form.append('rate', rateInput.value);
    form.append('delay', deliveryDelayInput.value);
    form.append('warranty', warrantyInput.value);
    form.append('clientName', clientNameInput.value);
    form.append('overAllAmount', total);
    form.append('products', products);

    let response = await fetch(url, {
        method: 'POST',
        body: form,
    });

    if (response.ok) {
        let result = await response.json();

        const convertedDocument = {}
        const documentType = getPaperType(result.paperType);

        convertedDocument.documentNumber = result.documentNumber;
        convertedDocument.creationDate = result.creationDate;
        convertedDocument.clientName = result.clientName;
        convertedDocument.type = documentType;
        convertedDocument.validityDate = result.validityDate;
        convertedDocument.rate = result.rate;
        convertedDocument.isPayable = result.isPayable;
        convertedDocument.warranty = result.warranty;
        convertedDocument.deliveryDelay = result.deliveryDelay;
        convertedDocument.productsDetails = prods;
        convertedDocument.totalAmount = result.totalAmount;
        convertedDocument.cashDeposit = result.cashDeposit ?? 0;
        convertedDocument.remainingBalance = result.remainingBalance ?? 0;

        await displayDocumentDetails(convertedDocument);

    } else {
        console.log('Response Error');
    }
}

//#endregion


// #region Get Products Function

function getProducts() {
    let products = [];

    const rows = tableBody.querySelectorAll('.table-body tr');

    for (const row of rows) {

        var otherInputs = row.querySelectorAll('input');

        let product = {
            description: row.querySelector('textarea').value,
        };
        otherInputs.forEach((element) => {
            product[`${element.name}`] = element.value;
        })
        products.push(product);
    }

    commercialDoc.productsDetails = products;

    return products;
}

// #endregion


// #region Overall Total Amount Calculation

function calculateOverallTotalAmount() {

    let totalInputs = document.querySelectorAll('input[id="product-total-price"]');
    if (totalInputs.length < 1) {
        return;
    }

    let sum = 0;

    for (const input of totalInputs) {
        sum += parseFloat(input.value);
    }

    return sum;
}

// #endregion


// #region Get Paper Type from Enum

function getPaperType(n) {
    switch (n) {
        case 1:
            return 'Quotation';
        case 2:
            return 'Invoice';
        case 3:
            return 'Receipt';
        case 4:
            return 'DeliveryNote';
        default:
            break;
    }
}

// #endregion

// #region Pay Invoice 

const payBtn = document.querySelector('.pay')
payBtn.addEventListener('click', () => {

    if (commercialDoc.documentNumber) {
        payInvoice();
    }

});

async function payInvoice() {

    alertMessage.textContent = "Are You Sure You Want To Close That Invoice";
    alertBox.style.display = 'flex';

    okButton.style.backgroundColor = "#2e7db4";

    okButton.onclick = () => {
        alertBox.style.display = 'none';
        pay();
    };
}

async function pay() {
    let id = commercialDoc.id;

    const url = `/api/pay/invoice/${id}`;

    let response = await fetch(url, {
        method: 'POST',
    });

    if (response.ok) {
        let result = await response.json();

        alert(result.message);
    } else {
        displayAlertMessage('Error while executing the payment' + '\nConsider Posting The Invoice First');
    }
}
// #endregion




