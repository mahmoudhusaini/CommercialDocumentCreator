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
    statementContainer.innerHTML = `${statement.template}`;
}

detailsStatementBtn.addEventListener('click', () => {
    detailedStatementContainer.style.display = 'block';
    detailedStatementContainer.innerHTML = `${statement.detailedTemplate}`;
});