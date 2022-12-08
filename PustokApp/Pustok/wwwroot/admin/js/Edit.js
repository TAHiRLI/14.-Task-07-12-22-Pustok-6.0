const btns = document.getElementsByClassName("close");
console.log(btns);

for (let item of btns) {
    item.addEventListener("click", (e) => {
        e.target.parentElement.removeChild(e.target.parentElement.lastElementChild)
        e.target.parentElement.style.display ="none";
    })
    
}