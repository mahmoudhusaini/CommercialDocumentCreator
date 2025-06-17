
const gridBtnsContainer = document.querySelector('.grid-btns-container');


async function loadQuotationPage() {
    window.location.href = '/clientapp/create-quotation/index.html';
}


async function loadInvoicePage() {
    window.location.href = '/clientapp/create-invoice/index.html';
}

async function loadReceiptPage() {
    window.location.href = '/clientapp/create-receipt/index.html';
}

async function loadNewProductPage() {
    window.location.href = '/clientapp/new-product/index.html';
}


gridBtnsContainer.addEventListener('click', (event) => {
    let button = event.target;

    switch (button.id) {

        case 'new-quotation':
            loadQuotationPage();
            break;
        case 'new-invoice':
            loadInvoicePage();
            break;
        case 'new-receipt':
            loadReceiptPage();
            break;
        case 'new-product':
            loadNewProductPage();
            break;
    }
});

const gridViewContainer = document.querySelector('#view-btns-container');

gridViewContainer.addEventListener('click', (event) => {
    let button = event.target;
    
    switch (button.id) {
        case 'view-quotation':
            loadViewQuotationsPage();
            break;
        case 'view-invoice':
            loadViewInvoicesPage();
            break;
        case 'view-receipt':
            loadViewReceiptsPage();
            break;

        default:
            break;
    }
});

async function loadViewInvoicesPage() {
    window.location.href = '/clientapp/view-invoices/index.html';
}

async function loadViewQuotationsPage() {
    window.location.href = '/clientapp/view-quotations/index.html';
}

async function loadViewReceiptsPage() {
    window.location.href = '/clientapp/view-reciepts/index.html';
}


const sidebar = document.getElementById('sidebar');
const toggleBtn = document.getElementById('toggleBtn');

toggleBtn.addEventListener('click', () => {
    sidebar.classList.toggle('closed');
});