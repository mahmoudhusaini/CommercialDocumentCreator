

// #region Variables

const loadingSpinner = document.getElementById("loadingSpinner");
const invoiceList = document.getElementById("invoiceList");

const alertBox = document.querySelector('#customAlert');
const alertMessage = document.querySelector('#alertMessage');

let okButton = document.querySelector('#alertOkButton');
const cancelButton = document.querySelector('#alertCancelButton');
cancelButton.onclick = () => {
    alertBox.style.display = 'none';
};

let singleInvoiceProducts = [];

let toggleSwitch = document.querySelector('#toggleStatus');
let invoicesLength = 0;

// #endregion



// #region ON LOAD EVENT

window.onload = spin();

// #endregion


// #region Toggle Switch


toggleSwitch.addEventListener('change', (e) => {

    if (e.target.checked) { //  const isChecked = this.checked;
        loadInvoices(true);
    } else {
        loadInvoices(false)
    }

});

// #endregion




// #region GET INVOICES COUNT
async function invoicesCountAppender() {
    let invoicesCountHolder = document.querySelector('.invoices-count h3');
    invoicesCountHolder.innerHTML = '';
    invoicesCountHolder.appendChild(document.createTextNode(`Total Invoices Count: ${invoicesLength}`));
}

// #endregion



// #region SPINNER FUNCTION

function spin() {

    setTimeout(() => {
        loadingSpinner.style.display = "none";
        loadInvoices();
        invoiceList.style.display = "block";
    }, 2000);
};

// #endregion


// #region Load Invoices function


async function loadInvoices(withPaid) {

    const url = `/api/all/invoices/${withPaid ?? false}`;

    let response = await fetch(url);

    if (response.ok) {

        let result = await response.json();
        invoicesLength = result.length;
        invoicesCountAppender();
        await displayInvoices(result);

    } else {
        console.log('Error fetching invoices');
    }

}


async function displayInvoices(invoices) {

    invoiceList.innerHTML = '';

    if (invoices.length <= 0) {

        let message = document.createElement('h3');
        message.className = 'no-invoices';

        message.appendChild(document.createTextNode("All invoices are cleared up till now"));
        invoiceList.appendChild(message);

        return;
    }

    for (const invoice of invoices) {
        let li = document.createElement('li');
        li.className = 'invoice-item';
        li.setAttribute('data-reference', invoice.id);

        invoiceList.appendChild(li);

        let headerInvoiceItem = document.createElement('div');
        li.appendChild(headerInvoiceItem);

        let span1 = document.createElement('span');
        let span2 = document.createElement('span');
        let span3 = document.createElement('span');

        span1.className = 'invoice-name';
        span2.className = 'invoice-amount';
        span3.className = 'invoice-currency';


        span1.appendChild(document.createTextNode(invoice.clientName));
        span2.appendChild(document.createTextNode(`\t${invoice.totalAmount}`));
        span3.appendChild(document.createTextNode(`\t$`));

        headerInvoiceItem.appendChild(span1);
        headerInvoiceItem.appendChild(span2);
        headerInvoiceItem.appendChild(span3);


        let extraInfoInvoiceItem = document.createElement('div');
        extraInfoInvoiceItem.className = 'invoice-extra-info';
        li.appendChild(extraInfoInvoiceItem);

        let pCreationDate = document.createElement('p');
        pCreationDate.appendChild(document.createTextNode(`Creation Date: ${invoice.creationDate.slice(0, 10)}`));

        let pValidityDate = document.createElement('p')
        pValidityDate.appendChild(document.createTextNode(`Validity Date: ${invoice.validityDate.slice(0, 10)}`));

        let pCahDeposit = document.createElement('p')
        pCahDeposit.appendChild(document.createTextNode(`Cash Deposit: ${invoice.cashDeposit}`));

        let pRemainingBalance = document.createElement('p')
        pRemainingBalance.appendChild(document.createTextNode(`Remaining Balance: ${invoice.remainingBalance}`));

        let pStatusState = document.createElement('p')
        pStatusState.appendChild(document.createTextNode(`State: ${getStatusState(invoice.status)}`));

        let pIsPosted = document.createElement('p')
        pIsPosted.appendChild(document.createTextNode(`Posted: ${invoice.isPosted}`));

        extraInfoInvoiceItem.appendChild(pCreationDate);
        extraInfoInvoiceItem.appendChild(pValidityDate);
        extraInfoInvoiceItem.appendChild(pCahDeposit);
        extraInfoInvoiceItem.appendChild(pRemainingBalance);
        extraInfoInvoiceItem.appendChild(pStatusState);
        extraInfoInvoiceItem.appendChild(pIsPosted);



        let buttonsContainer = document.createElement('div');
        buttonsContainer.className = 'invoice-buttons';
        li.appendChild(buttonsContainer);

        const deleteButton = document.createElement('button');
        deleteButton.textContent = 'Delete';
        deleteButton.classList.add('invoice-btn', 'delete-btn');

        const postButton = document.createElement('button');
        postButton.textContent = 'Post';
        postButton.classList.add('invoice-btn', 'post-btn');
        invoice.isPosted ? postButton.setAttribute('disabled', true) : postButton.setAttribute('enabled', true);


        const payButton = document.createElement('button');
        payButton.textContent = ' Pay';
        payButton.classList.add('invoice-btn', 'pay-btn');
        invoice.status == 2 ? payButton.setAttribute('disabled', true) : 1;

        const openButton = document.createElement('button');
        openButton.textContent = 'Open';
        openButton.classList.add('invoice-btn', 'open-btn');

        buttonsContainer.append(postButton, payButton, deleteButton, openButton);

    }
}

//#endregion



// #region Invoice Buttons Actions

invoiceList.addEventListener('click', (event) => {

    let target = event.target;
    if (target.tagName === 'BUTTON') {
        const parentItem = target.closest('.invoice-item');
        const invoiceId = parentItem.dataset.reference;

        switch (target.classList[1]) {
            case 'delete-btn':
                customDeleteAlert('Are you sure you want to delete this record!', invoiceId);
                break;
            case 'post-btn':
                showPostModal(invoiceId);
                break;
            case 'pay-btn':
                showPayModalDialog('Do you want to pay this invoice?', invoiceId);
                break;
            case 'open-btn':
                openPopUp(parentItem);
                break;
        }
    }

});



// #endregion

// #region Delete Invoice

async function deleteInvoice(id) {
    if (!id) {
        return;
    }
    const url = `/api/delete/invoice/${id}`;
    let response = await fetch(url, { method: 'DELETE' });
    if (response.ok) {
        let result = await response.json();
        
        location.reload();
    } else {
        console.log('error deleting invoice');
    }

};

function customDeleteAlert(message, invoiceId) {

    alertMessage.textContent = message;
    alertBox.style.display = 'flex';

    okButton.style.backgroundColor = "#a93226";

    okButton.onclick = () => {
        alertBox.style.display = 'none';
        deleteInvoice(invoiceId);
    };


}

// #endregion

// #region Pay Invoice


async function payInvoice(id) {
    const url = `/api/pay/invoice/${id}`;
    let response = await fetch(url,
        { method: 'POST', }
    );
    if (response.ok) {
        location.reload();
    } else {
        okButton.style.display = 'none';
        alertMessage.textContent = 'Error while executing the payment' + '\nConsider Posting The Invoice First';
        alertBox.style.display = 'flex';
    }
}

function showPayModalDialog(message, invoiceId) {

    alertMessage.textContent = message;
    alertBox.style.display = 'flex';

    okButton.style.display = 'inline';
    okButton.style.backgroundColor = "#2e7db4";

    okButton.onclick = () => {
        alertBox.style.display = 'none';
        payInvoice(invoiceId);
    };
}
// #endregion

// #region Post Invoice

async function showPostModal(id) {

    if (!id) {
        return;
    }


    const message = "Confirm that the order has been delivered?";
    alertMessage.textContent = message;
    alertBox.style.display = 'flex';

    okButton.style.display = 'inline';
    okButton.style.backgroundColor = "#2e7db4";

    okButton.onclick = () => {
        alertBox.style.display = 'none';
        postInvoice(id);
        location.reload();
    };


}

async function postInvoice(id) {

    const url = `/api/post/invoice/${id}`;

    try {
        let response = await fetch(url,
            { method: 'POST', }
        );

        if (response.ok) {
            let result = await response.json();
        } else {
            console.log('No response');
        }

    } catch {
        throw new Error("Server ERROR");
    }
}

// #endregion

// #region HIDE INVOICE

function hidePopupInvoice() {
    let popup = document.querySelector('#invoice-popup');
    if (popup) {
        popup.remove();
    }
}

// #endregion

// #region Open Invoice

async function openPopUp(invoiceItem) {
    const id = invoiceItem.dataset.reference;

    if (!id) {
        return;
    }


    let invoiceChildNodes = invoiceItem.childNodes;
    let amount = invoiceChildNodes[0].querySelector('.invoice-amount').textContent;

    // let companyName = invoiceChildNodes[0].querySelector('.invoice-name').textContent;
    // let creationDate = invoiceChildNodes[1].querySelectorAll('p')[0].textContent;
    // let validityDate = invoiceChildNodes[1].querySelectorAll('p')[1].textContent;
    // let cashDeposit = invoiceChildNodes[1].querySelectorAll('p')[2].textContent;
    // let remainingBalance = invoiceChildNodes[1].querySelectorAll('p')[3].textContent;
    // let status = invoiceChildNodes[1].querySelectorAll('p')[4].textContent;

    const url = `/api/get/invoice/${id}`;

    let invoice = undefined, details = [];

    try {
        let response = await fetch(url, {
            method: 'GET',
        });

        if (response.ok) {
            let result = await response.json();
            invoice = result.invoice;
            details = JSON.parse(result.details);
            singleInvoiceProducts = details;
        } else {
            console.log('No response');
        }

    } catch (error) {
        throw new Error("Server ERROR: " + error.message);
    }


    let detailsTemplate = '';

    for (let i = 0; i < details.length; i++) {
        detailsTemplate += `
        
            <div class="detail-item">
                <span class="detail-name">${details[i].name}</span>
                <span class="detail-separator">|</span>
                <span class="detail-prop">Description: ${details[i].description}</span>
                <span class="detail-prop">Price: ${details[i].sellingPrice}</span>
                <span class="detail-prop">Qty: ${details[i].quantityInStock}</span>
                <span class="detail-prop">Total: ${details[i].productTotalPrice}</span>
            </div>
`;

    }


    let popUpTemplate = ` 
    <div class="popup-overlay" id="invoice-popup">
        <div class="popup-content">
            <h3>Invoice: ${invoice.documentNumber}</h3>

            <p>
               <strong>Client:</strong>
               <input class="clientNameInput" type="text" required value="${invoice.clientName}">
            </p>

            <p>
                <strong>Delivery Delay:</strong>
                <input class="deliveryDelayInput" type="text" required value="${invoice.deliveryDelay}">
            </p>
            
            <p>
                <strong>Deposit:</strong>
                <input class="cashDepositInput" type="text" required value="${invoice.cashDeposit}">
            </p>
            
            <p>
                <strong>Rate:</strong>
                <input class="rateInput" type="text" required value="${invoice.rate}">
            </p>
            
            <p>
                <strong>Warranty:</strong>
                <input class="warrantyInput" type="text" required value="${invoice.warranty}" required>
            </p>
            
            <br>
            <h3>Details:</h3>

           <div class="products-holder" id="details-container">
                ${detailsTemplate}
           </div>
           
           <button class="add-detail-btn" type="button" onclick="addDetailRow()">+ Add Item</button>


            <div class="btns-holder">
                <button class="close-btn" onclick="hidePopupInvoice()">Close</button>
                <button class="update-btn" onclick="UpdateInvoice(${id}, ${amount})" >Update</button>
            </div>

        </div>
    </div>
`;

    document.body.insertAdjacentHTML('beforeend', popUpTemplate);

    document.querySelector('#invoice-popup').style.display = 'flex';
}

// #endregion

// #region UPDATE INVOICE

async function UpdateInvoice(id, amount) {

    let addedAmount = await updateProductsAndTotal();

    let clientNameInput = document.querySelector('.clientNameInput');
    let delayInput = document.querySelector('.deliveryDelayInput');
    let cashDepositInput = document.querySelector('.cashDepositInput');
    let rateInput = document.querySelector('.rateInput');
    let warrantyInput = document.querySelector('.warrantyInput');
    let products = JSON.stringify(singleInvoiceProducts);
    let overAllAmount = addedAmount + amount;

    let form = new FormData();

    form.append('id', id);
    form.append('client', clientNameInput.value);
    form.append('delay', delayInput.value);
    form.append('cashDeposit', cashDepositInput.value);
    form.append('rate', rateInput.value);
    form.append('warranty', warrantyInput.value);
    form.append('products', products);
    form.append('overAllAmount', overAllAmount);

    const url = `/api/Create/Invoice`;
    let response = await fetch(url, {
        method: 'POST',
        body: form,
    });

    if (response.ok) {
        let result = await response.json();


        location.reload();

    } else {
        console.log('error');
    }

}

// #endregion


// #region ADD PRODUCT ROW


function addDetailRow() {
    const container = document.getElementById("details-container");
    const row = document.createElement("div");
    row.className = "detail-row";

    row.innerHTML = `
        <input type="text" placeholder="Name" class="detail-input name" required />
        <input type="text" placeholder="Description" class="detail-input desc" />
        <input type="number" placeholder="Price" class="detail-input price" min="0" step="0.01" />
        <input type="number" placeholder="Qty" class="detail-input qty" min="0" step="1" />
        <input type="number" placeholder="Total" class="detail-input total" readonly />
        <button type="button" onclick="removeDetailRow(this)">❌</button>
    `;

    const priceInput = row.querySelector('.price');
    const qtyInput = row.querySelector('.qty');
    const totalInput = row.querySelector('.total');

    function calculateTotal() {
        const price = parseFloat(priceInput.value) || 0;
        const qty = parseFloat(qtyInput.value) || 0;
        const total = price * qty;
        totalInput.value = total >= 0 ? total.toFixed(2) : "0.00";
    }

    priceInput.addEventListener('input', calculateTotal);
    qtyInput.addEventListener('input', calculateTotal);

    container.appendChild(row);
}

// #endregion

// #region REMOVE ROW

function removeDetailRow(button) {
    button.parentElement.remove();
}

//#endregion


// #region UPDATE PRODUCTS LIST AND INVOICE TOTAL

async function updateProductsAndTotal() {

    let rows = document.querySelectorAll('.detail-row');
    let addedOverAllTotal = 0;

    for (const row of rows) {

        let nameInput = row.querySelector('.name');
        let nameValue = nameInput?.value ?? '';

        let descInput = row.querySelector('.desc');
        let description = descInput?.value ?? '';

        let priceInput = row.querySelector('.price');
        let price = priceInput?.value ?? 0;

        let qtyInput = row.querySelector('.qty');
        let qty = qtyInput?.value ?? 0;

        let totalInput = row.querySelector('.total');
        let total = parseFloat(totalInput?.value) ?? 0;

        addedOverAllTotal += total;

        let newProduct = {
            description: description,
            name: nameValue,
            productTotalPrice: total,
            quantityInStock: qty,
            sellingPrice: price,
        }
        singleInvoiceProducts.push(newProduct);

    }
    return addedOverAllTotal;
}

//#endregion



// #region Get Payment Status

function getStatusState(status) {
    switch (status) {
        case 0:
            return "Unpaid";
        case 1:
            return "Paid Partially";

        case 2:
            return "Paid Completely";
        default:
            break;
    }
}
// #endregion




// What is FindAsync in EF Core is it deffered execution??
// recheck on operations logic like try catch-check ids etc....
// httpdelete | httpget
// clgs to fullfill