let categories = [];

async function getCategories(params) {
    const url = '/api/categories/all';
    let response = await fetch(url);
    
    if(response.ok){
        let result = await response.json();
        console.log(result);
        categories = result;
        console.log(categories);
    } else {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Something went wrong!",
            // footer: '<a href="#">Why do I have this issue?</a>'
          });
    }
}
 getCategories();



function toggleParentCategory() {
    let categoryLevel = document.getElementById("categoryLevel").value;
    let parentCategoryDiv = document.getElementById("parentCategoryDiv");

    if (categoryLevel > 1) {
        parentCategoryDiv.classList.remove("hidden");
        loadParentCategories();
    } else {
        parentCategoryDiv.classList.add("hidden");
        document.getElementById("parentCategory").value = 0;
    }
}

function loadParentCategories() {
    let parentCategorySelect = document.getElementById("parentCategory");
    parentCategorySelect.innerHTML = '<option value="">Select Parent Category</option>';

    categories.forEach(category => {
        let option = document.createElement("option");
        option.value = category.id;
        option.textContent = category.categoryName;
        parentCategorySelect.appendChild(option);
    });
}
// if parent is 0 level must be main(1)
document.getElementById("categoryForm").addEventListener("submit", function (event) {
    event.preventDefault();

    let categoryName = document.getElementById("categoryName").value;
    let categoryLevel = document.getElementById("categoryLevel").value;
    let parentCategoryID = document.getElementById("parentCategory").value || 0;
    
    if (categoryLevel > 1 && !parentCategoryID) {
        alert("Please select a Parent Category for subcategories.");
        return;
    }

    let categoryObject = {
        categoryName,
        categoryLevel,
        parentCategoryID: categoryLevel == 1 ? 0 : parseInt(parentCategoryID),
    };
    generateCategory(categoryObject);

});

async function generateCategory(categoryObject) {
    const url = `/api/categories/new`;
 
    let categoryName = categoryObject.categoryName;
    let categoryLevel = categoryObject.categoryLevel;
    let parentCategoryID = categoryObject.parentCategoryID;
    
    let form = new FormData();
    form.append('categoryName', categoryName);
    form.append('categoryLevel', categoryLevel);
    form.append('parentCategoryID', parentCategoryID);

    try {
             let response = await fetch(url, {
                method:'POST',
                body: form,
             });
             if (response.ok) {
                 let result = await response.json();
                Swal.fire(`${result.message}`, 'success');
                await getCategories();

             } else {
                 console.log('error');
             }
        } catch (error) {
            console.log(error);
        }
}

