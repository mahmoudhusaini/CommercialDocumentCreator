const shipmentTitleInput = document.querySelector('#shipment-title');

// #region Add Table Row (Product Row)
function addProduct() {
    let tableContainer = document.getElementById("tableContainer");
    let tbody = document.getElementById("productTableBody");

    if (tbody.children.length === 0) {
        tableContainer.style.display = "block"; // Show table when first product is added
    }

    let row = document.createElement("tr");
   
    row.innerHTML = `
        <td><input type="text" class="product-name"></td>
        <td><input type="number" min="1" class="qty" value="1" oninput="calculateRow(this)"></td>
        <td><input type="number" min="1" class="unit-price" value="1" oninput="calculateRow(this)"></td>
        <td><input type="number" class="total-price" value="0" readonly></td>
        <td><input type="number" class="weight" value="0"></td>
        <td><input type="number" min="1" class="freight-weight" value="1" oninput="calculateRow(this)"></td>
        <td><input type="number" class="total-cost" value="1" readonly></td>
        <td><input type="number" class="landed-cost" value="1" readonly></td>
        <td><input type="checkbox" class="delivered-checkbox"></td>
        <td><button class="remove-button" onclick="removeRow(this)">X</button></td>
        <td><button class="reset-button" onclick="resetRow(this)">Reset</button></td>
    `;

    tbody.appendChild(row);
}
// #endregion

// #region Calculate Row

function calculateRow(inputElement) {
    if(inputElement.value < 0){
        input.value = 1;
        return;
    }
    let row = inputElement.closest("tr");

    let qty = parseInt(row.querySelector(".qty").value) || 0;
    let unitPrice = parseFloat(row.querySelector(".unit-price").value) || 0;
    let totalPrice = qty * unitPrice;
    row.querySelector(".total-price").value = totalPrice.toFixed(2);

    let weight = parseFloat(row.querySelector(".freight-weight").value) || 0;
    let freightRate = parseFloat(document.querySelector(".freight-rate").value) || 0;
    let weightCost = freightRate * weight;

    let transferPercentage = document.getElementById("transfer").value / 100;
    let totalCost = (totalPrice + weightCost) * (1 + transferPercentage);
    let landedCost = totalCost / qty; 

    row.querySelector(".total-cost").value = totalCost.toFixed(2);
    row.querySelector(".landed-cost").value = landedCost.toFixed(2);
}
// #endregion

// #region Delete Button 

function removeRow(button) {
    let row = button.closest("tr");
    row.remove();

    let tbody = document.getElementById("productTableBody");
    if (tbody.children.length === 0) {
        document.getElementById("tableContainer").style.display = "none"; // Hide table if no products
    }
}
// #endregion

// #region Reset Button 

function resetRow(target){
    let currentTr = target.closest('tr');
    let inputs = currentTr.querySelectorAll('input');
    for(input of inputs){
        input.value = 0;
    }
    let checkbox = document.querySelector('[type="checkbox"]');
    checkbox.checked =false
}
// #endregion

async function printShipment(){
    const table = document.querySelector("#productTableBody"); 
    let trs = table.querySelectorAll("tr");
    let products = [];
    for (const tr of trs) {
      let allInputs =  tr.querySelectorAll('input');

        let object = {};
        for(let i = 0; i < allInputs.length; i++){
            if(allInputs[i].type === 'checkbox'){
                object[allInputs[i]["className"]] = allInputs[i].checked;

            } else {
                object[allInputs[i]["className"]] = allInputs[i].value;
            }
        }
        products.push(object);
    }
    let percentage = (document.getElementById("transfer").value / 100) + 1  || 0;
    let freightRate =  parseFloat(document.querySelector(".freight-rate").value) || 0;
    let shipmentTitle = shipmentTitleInput.value;
    const url = `/api/tradetally/validate/shipment/${percentage}/${freightRate}/${shipmentTitle}`;
   
    try{
        let response = await fetch(url, {
            method:'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(products),
        });

        if(response.ok){
            let result = await response.json();
           
            // const newWindow = window.open("", "_blank"); 
            // newWindow.document.write(result.content);
            // newWindow.document.close();
            // newWindow.print(); 
            // window.document.write(result.content);
            // window.document.close();
            // window.print(); 

            
        
            // function downloadBlob(blobData, fileName, dataType) {
            //     let blob = new Blob([blobData], { type: dataType });
            //     let url = URL.createObjectURL(blob);
            //     const link = document.createElement('a');
            //     link.href = url;
            //     link.download = `${fileName}.csv`;
        
            //     link.click();
            // }
            
        } else {
            console.log('error');
        }
    } catch (error) {
        console.log(error);
    }
}



