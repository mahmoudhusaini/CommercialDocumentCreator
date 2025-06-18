

// #region Variables

const loadingSpinner = document.getElementById("loadingSpinner");
const recieptsList = document.getElementById("recieptsList");

const alertBox = document.querySelector('#customAlert');
const alertMessage = document.querySelector('#alertMessage');
let okButton = document.querySelector('#alertOkButton');

const cancelButton = document.querySelector('#alertCancelButton');
cancelButton.onclick = () => {
    alertBox.style.display = 'none';
};

let singleRecieptProducts = [];

let toggleSwitch = document.querySelector('#toggleStatus');
let recieptsLength = 0;

// #endregion


// #region CUSTOM ALERT

function customAlert(message) {
    alertMessage.textContent = message;
    alertBox.style.display = 'flex';


    okButton.style.backgroundColor = "#a93226";

    okButton.onclick = () => {
        alertBox.style.display = 'none';
    };
}

// #endregion




// #region ON LOAD EVENT

window.onload = spin();

// #endregion


// #region SPINNER FUNCTION

function spin() {

    setTimeout(() => {
        loadingSpinner.style.display = "none";
        loadReciepts();
        recieptsList.style.display = "block";
    }, 2000);
};

// #endregion


// #region Load Reciepts function


async function loadReciepts() {

    const url = `/api/All/Reciepts`;

    let response = await fetch(url, { method: 'GET' });

    if (response.ok) {

        let result = await response.json();
        recieptsLength = result.length;
        await displayReciepts(result);

    } else {
        console.log('Error fetching reciepts');
    }

}


async function displayReciepts(reciepts) {

    recieptsList.innerHTML = '';

    if (reciepts.length <= 0) {

        let message = document.createElement('h3');
        message.className = 'no-reciepts';

        message.appendChild(document.createTextNode("No Available Reciepts"));
        recieptsList.appendChild(message);

        return;
    }

    for (const reciept of reciepts) {
        let li = document.createElement('li');
        li.className = 'reciept-item';
        li.setAttribute('data-reference', reciept.id);

        recieptsList.appendChild(li);

        let headerRecieptItem = document.createElement('div');
        li.appendChild(headerRecieptItem);

        let span1 = document.createElement('span');
        let span2 = document.createElement('span');
        let span3 = document.createElement('span');

        span1.className = 'reciept-name';
        span2.className = 'reciept-amount';
        span3.className = 'reciept-currency';


        span1.appendChild(document.createTextNode(reciept.clientName));
        span2.appendChild(document.createTextNode(`\t${reciept.totalAmount}`));
        span3.appendChild(document.createTextNode(`\t$`));

        headerRecieptItem.appendChild(span1);
        headerRecieptItem.appendChild(span2);
        headerRecieptItem.appendChild(span3);


        let extraInfoRecieptItem = document.createElement('div');
        extraInfoRecieptItem.className = 'reciept-extra-info';
        li.appendChild(extraInfoRecieptItem);

        let pCreationDate = document.createElement('p');
        pCreationDate.appendChild(document.createTextNode(`Creation Date: ${reciept.creationDate.slice(0, 10)}`));

        let pValidityDate = document.createElement('p')
        pValidityDate.appendChild(document.createTextNode(`Validity Date: ${reciept.validityDate.slice(0, 10)}`));

        let bDocumentNumber = document.createElement('b')
        bDocumentNumber.appendChild(document.createTextNode(`Document Number: ${reciept.documentNumber}`));

        extraInfoRecieptItem.appendChild(bDocumentNumber);
        extraInfoRecieptItem.appendChild(pCreationDate);
        extraInfoRecieptItem.appendChild(pValidityDate);


        let buttonsContainer = document.createElement('div');
        buttonsContainer.className = 'quotation-buttons';
        li.appendChild(buttonsContainer);

        const deleteButton = document.createElement('button');
        deleteButton.textContent = 'Delete';
        deleteButton.classList.add('quotation-btn', 'delete-btn');

        const openButton = document.createElement('button');
        openButton.textContent = 'Open';
        openButton.classList.add('quotation-btn', 'open-btn');

        buttonsContainer.append(deleteButton, openButton);
    }
}

//#endregion



// #region Buttons Actions

recieptsList.addEventListener('click', (event) => {

    let target = event.target;
    if (target.tagName === 'BUTTON') {
        const parentItem = target.closest('.reciept-item');
        const quotationId = parentItem.dataset.reference;

        switch (target.classList[1]) {
            case 'delete-btn':
                customDeleteAlert('Are you sure you want to delete this record!', quotationId);
                break;
            case 'open-btn':
                const id = parentItem.dataset.reference;

                openPopUp(id);
                break;
        }
    }

});



// #endregion

// #region Delete QUOTATION

async function deleteReciept(id) {
    if (!id) {
        return;
    }
    const url = `/api/delete/reciept/${id}`;
    let response = await fetch(url, { method: 'DELETE' });
    if (response.ok) {
        //let result = await response.json();
        location.reload();

    } else {
        console.log('error deleting reciept');
    }

};

function customDeleteAlert(message, id) {

    alertMessage.textContent = message;
    alertBox.style.display = 'flex';

    okButton.style.backgroundColor = "#a93226";

    okButton.onclick = () => {
        alertBox.style.display = 'none';
        deleteReciept(id);
    };

}

// #endregion




// #region HIDE QUOTATION

function closePopup() {
    let popup = document.querySelector('.popup-overlay');
    if (popup) {
        popup.remove();
    }
}

// #endregion

// #region OPEN QUOTATION

async function openPopUp(id) {

    if (!id) {
        return;
    }

    const url = `/api/get/receipt/${id}`;

    let quotation = undefined, details = [], total = 0;

    try {
        let response = await fetch(url, {
            method: 'GET',
        });

        if (response.ok) {
            let result = await response.json();

            quotation = result.receipt;
            details = JSON.parse(result.details);
            singleRecieptProducts = details;
            total = parseFloat(quotation.totalAmount);

        } else {
            console.log('No response');
        }

    } catch (error) {
        throw new Error("Server ERROR: " + error.message);
    }



    let detailsTemplate = '';



    for (let i = 0; i < details.length; i++) {
        detailsTemplate += `
        
            <div class="product-row">
                <div>
                    <input disabled class="det-inp" type="text" value="${details[i].name}" />
                </div>
                <div>
                   <textarea disabled class="prod-det-txtarea">${details[i].description}</textarea>
                </div>
                
                <div>
                    <input disabled type="number" value="${details[i].sellingPrice}" />
                </div>
                
                <div>
                    <input disabled type="number" value="${details[i].quantityInStock}" />
                </div>
                
                <div>${details[i].productTotalPrice}</div>
            </div>
`;

    }



    let popUpTemplate = ` 
<div class="popup-overlay" style="display: none;">
  <div class="popup-content">
    
    <button class="close-popup-btn" onclick="closePopup()">✖</button>

    <div class="popup-sidebar">
      <label for="client-name">Client Name:</label>
      <input type="text" value="${quotation.clientName}" id="client-name" name="client-name" placeholder="Enter client name" />

      <label for="total">Total:</label>
      <input type="text" value="${quotation.totalAmount}" id="total" name="total" readonly />

      <label for="warranty">Warranty:</label>
      <input type="text" value="${quotation.warranty}" id="warranty" name="warranty" />

      <label for="rate">Rate:</label>
      <input type="number" value="${quotation.rate}" id="rate" name="rate" />

      <label for="delivery-delay">Delay:</label>
      <input type="number" value="${quotation.deliveryDelay}" id="delivery-delay" name="delivery-delay" />

      <button class="update-btn" onclick="UpdateQuotation(${id}, ${total})">Update</button>
    </div>
    
    <div class="popup-products">
      
      <div class="product-table">
         <div class="product-row header">
            <div>Product</div>
            <div>Description</div>
            <div>Price</div>
            <div>Qty</div>
            <div>Total</div>
            <div>Actions</div>
         </div>

        ${detailsTemplate}
      </div>
      <div>
            <button class="add-detail-btn" type="button" onclick="addDetailRow()">+ Add Item</button>
      </div>
    </div>
  </div>
</div>

`;

    document.body.insertAdjacentHTML('beforeend', popUpTemplate);
    document.querySelector('.popup-overlay').style.display = 'flex';

}



// #endregion

// #region UPDATE QUOTATION

async function UpdateQuotation(id, amount) {

    let addedAmount = await updateProductsAndTotal();
    let overAllAmount = addedAmount + amount;

    let totalInput = document.querySelector('#total');
    totalInput.value = overAllAmount;

    let clientNameInput = document.querySelector('#client-name');
    let warrantyInput = document.querySelector('#warranty');
    let rateInput = document.querySelector('#rate');
    let delayInput = document.querySelector('#delivery-delay');

    let products = JSON.stringify(singleRecieptProducts);


    let form = new FormData();

    form.append('id', id);
    form.append('client', clientNameInput.value);
    form.append('delay', delayInput.value);
    form.append('rate', rateInput.value);
    form.append('warranty', warrantyInput.value);
    form.append('products', products);
    form.append('overAllAmount', overAllAmount);

    const url = `/api/Create/Receipt`;
    let response = await fetch(url, {
        method: 'POST',
        body: form,
    });

    if (response.ok) {
        location.reload();

    } else {
        console.log('error');
    }

}

// #endregion


// #region ADD PRODUCT ROW


function addDetailRow() {
    const container = document.querySelector(".product-table");
    const row = document.createElement("div");
    row.className = "new-product-row product-row";

    row.innerHTML = `
       
        
                <div>
                    <input class="det-inp name" type="text" placeholder="Name" required />
                </div>
                <div>
                   <textarea class="prod-det-txtarea desc" placeholder="Description"> </textarea>
                </div>
                
                <div>
                    <input class="price" type="number" placeholder="Price" min="0" step="0.01" />
                </div>
                
                <div>
                    <input class="qty" type="number" placeholder="Qty" min="0" step="1" />
                </div>
                
                 <div>
                    <input type="text" class="total" readonly />
                 </div>

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


// #region UPDATE PRODUCTS LIST AND QUOTATION TOTAL

async function updateProductsAndTotal() {

    let rows = document.querySelectorAll('.new-product-row');
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
        singleRecieptProducts.push(newProduct);
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


