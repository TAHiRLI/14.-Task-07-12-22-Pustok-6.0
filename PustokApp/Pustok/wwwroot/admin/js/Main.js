$(document).on("click", ".trash-btn", function (e) {
    e.preventDefault();

    let link = e.target.getAttribute("href");

    

    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    })
        .then((result) => {
            if (result.isConfirmed) {

                fetch(link)
                    .then(res => {

                        if (res.ok) {
                            Swal.fire(
                                'Deleted!',
                                'Your file has been deleted.',
                                'success'
                            ).then(() => window.location.reload())
                        }
                        else if (!res.ok) {
                            Swal.fire({
                                icon: 'error',
                                title: 'Oops...',
                                text: 'Something went wrong!',
                            }).then(() => window.location.reload())
                        }
                    })



           
        }
    })
})