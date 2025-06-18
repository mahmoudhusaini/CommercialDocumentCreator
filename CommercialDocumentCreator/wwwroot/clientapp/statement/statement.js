

let generateBtn = document.querySelector('.gen-statement');
let detailsStatementBtn = document.querySelector('.det-statement');
const startDateInput = document.querySelector('#start-date');
const endDateInput = document.querySelector('#end-date');
let statementContainer = document.getElementById('statement-result');
let printBtn = document.querySelector('.print-statement');
let detailedStatementContainer = document.querySelector('.detailed-statement-result');


let statement = {};

async function generateStatement(start, end) {

    let url = `/api/get/statement/${start}/${end}`;

    let response = await fetch(url);

    if (response.ok) {
        let result = await response.json();

        statement = result;

        await displayStatement();
    } else {
        console.log('Statement - Error');
    }
}


generateBtn.addEventListener('click', (event) => {

    let start = startDateInput.value;
    let end = endDateInput.value;

    if (start && end) {

        generateStatement(start, end);

    } else {
        statementContainer.innerText =
            `Please provide both dates.`;
    }
});


async function displayStatement() {

    if (statement === null || statement === undefined) {
        return;
    }

    detailsStatementBtn.style.display = 'block';
    
    
    statementContainer.innerHTML =
        `
                <div class="statement-summary">
                <h3>Statement Summary</h3>
                <p><strong>${statement.date}</strong></p>

                <p><strong>Period:</strong> ${statement.from} to ${statement.to}</p>

                <table class="statement-table">
                    <thead>
                        <tr>
                            <th>Description</th>
                            <th>Amount ($)</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>Cash From Receipts</td>
                            <td>${statement.cashFromReceipts}</td>
                            <td>
                                <a href="/clientapp/view-reciepts/index.html">
                                 <div class="green-circle"></div></td>
                                </a>
                            </td>
                        </tr>
                        <tr>
                            <td>Deposit From Invoices</td>
                            <td>${statement.depositFromInvoice}</td>
                            <td><div class="green-circle"></div></td>
                        </tr>
                        <tr>
                            <td>Pending From Invoices</td>
                            <td>${statement.pendingFromInvoice}</td>
                            <td>
                              <a href="/clientapp/view-invoices/index.html">
                                 <div class="red-circle"></div></td>
                              </a>
                        </tr>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td colspan="2"><strong>Total Income</strong></td>
                            <td colspan="2"><strong>${statement.totalIncome}</strong></td>
                        </tr>
                    </tfoot>
                </table>
            </div>
           
            `;
}

detailsStatementBtn.addEventListener('click', () => {
    detailedStatementContainer.style.display = 'block';
    detailedStatementContainer.innerHTML = `${statement.detailedTemplate}`;
});