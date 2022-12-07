const modalBtns = document.getElementsByClassName("modal-btn");
const modalContent = document.querySelector('.modal-dialog');
for (const btn of modalBtns) {
    btn.addEventListener("click", (e) => {
        e.preventDefault();
        let link = btn.getAttribute("href");
        console.log(link);


        fetch(link)
        .then(response => response.text())
        .then(data=> {
            console.log(data)
            modalContent.innerHTML = data;
         })

    })
}