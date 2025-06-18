let navConatiner = document.querySelector('.nav');

// #region Assemble Left Nav to The Nav Bar - Left Nav : Logo + Legal Info

let leftNav = document.createElement('div');
leftNav.className = 'left-nav';
navConatiner.appendChild(leftNav);

let imageContainer = document.createElement('div');
leftNav.appendChild(imageContainer);

const mainPageRef = '/clientapp/home/home.html';
let a = document.createElement('a');
a.setAttribute('href', mainPageRef);
imageContainer.appendChild(a);

let img = document.createElement('img');
img.className = 'company-logo';
img.setAttribute('src', '/clientapp/images/ictech-2-2.jpg');
a.appendChild(img);

let legalInfoContainer = document.createElement('div');
legalInfoContainer.className = 'legal-info';

leftNav.appendChild(legalInfoContainer);

let idNat = document.createElement('div');
idNat.className = 'idnat';
const idNatextNode = document.createTextNode("ID NAT : 01-F4300-N03397F");
idNat.appendChild(idNatextNode);
legalInfoContainer.appendChild(idNat);

let rccm = document.createElement('div');
rccm.className = 'rccm';
const rccmTextNode = document.createTextNode("RCCM : 22-A-01980");
rccm.appendChild(rccmTextNode);
legalInfoContainer.appendChild(rccm);

let nimpot = document.createElement('div');
nimpot.className = 'nimpot';
const nimpotTextNode = document.createTextNode("N Impot : A2205769W");
nimpot.appendChild(nimpotTextNode);
legalInfoContainer.appendChild(nimpot);

// #endregion

// #region Assemble Right Nav to The Nav Bar

let rightNav = document.createElement('div');
rightNav.className = 'right-nav';
navConatiner.appendChild(rightNav);

const partnershipsLogos = ['dell.jpg', 'hp.jpg', 'lenovo.jpg', 'apc.png', 'hikvision.jpg'];

for (let i = 0; i < 5; i++) {
    let div = document.createElement('div');
    rightNav.appendChild(div);

    let partnershipsLogo = document.createElement('img');
    partnershipsLogo.setAttribute('src', `/clientapp/images/${partnershipsLogos[i]}`);
    partnershipsLogo.className = `partnershipsLogo${i}`;
    div.appendChild(partnershipsLogo);
}
// #endregion


// #region Assemble Company Info 

let contentBelowNav = document.querySelector('.content-one');

if(contentBelowNav){
    assembleCompanyInfo();
}

function assembleCompanyInfo() {
    let contentOneLeft = document.createElement('div');
    contentOneLeft.className = 'content-one-left';
    contentBelowNav.appendChild(contentOneLeft);

    const companyInfo = ['Ets ICTech', 'South, Saida, Main Street - Lebanon',
        'Bank : SOFIBANQUE SA RDC', 'Account : 000232011002143080200-17 USD',
        'SWIFT : SFBXCDKIXXX', 'Email: ictech.congo@gmail.com', 'Phone Number: +961 76095672']

    for (let i = 0; i < 7; i++) {
        let p = document.createElement('p');

        const textNode = document.createTextNode(companyInfo[i]);
        p.appendChild(textNode);
        contentOneLeft.appendChild(p);
    }
}
//#endregion
