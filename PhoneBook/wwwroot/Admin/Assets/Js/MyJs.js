//#region  delete
var DelButton = document.querySelectorAll("#deleteItem");

if (DelButton != null) {
  DelButton.forEach((btn) =>
    btn.addEventListener("click", async function (b) {
      b.preventDefault();

      let url = btn.getAttribute("href");
      var SuperAdmin = await IsSuperAdmin();

      if (SuperAdmin == true && url.includes("PhoneManage")) {
        await Swal.fire({
          showDenyButton: true,
          showCancelButton: true,
          confirmButtonText: "Hard Delete",
          confirmButtonColor: "#ff0000",
          denyButtonText: `Soft Delete`,
          denyButtonColor: "#ff6a00",
        }).then((result) => {
          if (result.isConfirmed) {
            url = url.replace("delete", "HardDelete");
          } else if (result.isDenied) {
            url = url;
          } else if (result.dismiss === Swal.DismissReason.cancel) {
            return;
            return;
          }
        });
      }
      await Swal.fire({
        title: "əminsiniz",
        text: "bunu geri qaytara bilməyəcəksiniz",
        icon: "warning",
        showCancelButton: true,
        cancelButtonColor: "#3085d6",
        confirmButtonColor: "#d33",
        confirmButtonText: "Bəli, sil",
        cancelButtonText: "Ləğv et",
      }).then((result) => {
        if (result.isConfirmed) {
          fetch(url, {
            method: "DELETE",
          })
            .then((res) => res.json())
            .then((data) => {
              if (data.isSuccess) {
                console.log(data);
                const parent = btn.closest("tr");

                if (parent != null && SuperAdmin == false) {
                  parent.remove();
                }
                Swal.fire({
                  title: "Silindi!",
                  text: data.message,
                  icon: "success",
                });
              } else {
                Swal.fire({
                  title: "Xəta!",
                  text: data.message,
                  icon: "error",
                });
              }
            });
        }
      });
    })
  );
}

//#endregion soft delete

//#region reset page
var resetButton = document.querySelector("#resetBtn");

if (resetButton != null) {
  resetButton.addEventListener("click", async function (b) {
    b.preventDefault();
    window.location.reload();
  });
}

//#endregion reset

//#region  Delete Image
var deleteImageBtn = document.getElementById("deleteImageBtn");

if (deleteImageBtn != null) {
  deleteImageBtn.addEventListener("click", function (btn) {
    const image = document.getElementById("personImage");
    image.src = "/Uploads/PersonImages/default.jpg";
    var imageInput = document.getElementById("PersonImageInput");
    if (imageInput != null) {
      imageInput.value = null;
    }

    var deleteImageInput = document.getElementById("DeletedImage");
    if (deleteImageInput != null) {
      deleteImageInput.checked = true;
    }
    deleteImageBtn.style.display = "none";
  });
}

//#endregion DeleteImage

//#region  AddImage

var PersonImageInput = document.getElementById("PersonImageInput");

if (PersonImageInput != null) {
  PersonImageInput.addEventListener("change", function () {
    var selectedFile = PersonImageInput.files[0];

    if (selectedFile) {
      if (selectedFile.type.startsWith("image/")) {
        var imgElement = document.createElement("img");
        imgElement.classList.add("img-fluid");

        var reader = new FileReader();
        reader.onload = function (e) {
          var existingImgElement = document.getElementById("personImage");

          if (existingImgElement) {
            existingImgElement.src = e.target.result;
          }
        };
        reader.readAsDataURL(selectedFile);
      }
    }
  });
}

//#endregion AddImage

//#region  search for Admin

var searchInput = document.getElementById("searchInputForSA");

if (searchInput != null) {
  searchInput.addEventListener("input", function () {
    const baseUrl = document.getElementById("searchForm");
    if (baseUrl) {
      if (searchInput.value == "") {
        window.location.reload();
      }
      const url = baseUrl.action + "?searchQuery=" + searchInput.value;
      fetch(url, {
        method: "GET",
      })
        .then((res) => res.json())
        .then((data) => {
          if (data) {
            const tableBody = document.getElementById("tableBody");
            tableBody.innerHTML = "";
            var counter = 1;
            data.forEach((item) => {
              if (item.email == null) {
                item.email = "";
              }
              var row = `<tr> 
                    <td>${counter}</td>
                    <td>${
                      item.firstName +
                      " " +
                      item.lastName +
                      " " +
                      item.fatherName
                    }</td>
                    <td>${item.phoneNumber}</td>
                    <td>${item.internalNumber}</td>
                    <td>${item.email}</td>
                    <td>${item.address}</td>
                    <td>${item.position}</td>
                    
                    <td>
                        <a href="/Admin/PhoneManage/Update/${item.id}">
                            <button class="btn btn-warning">
                                <i class="far fa-edit"></i>
                            </button>
                        </a>

                        <a href="/Admin/PhoneManage/HardDelete/${item.id}" >
                                    <button class="btn btn-danger">
                                        <i class=" fas fa-trash-alt"></i>

                                    </button>
                                </a>
                    </td>

                  </tr > `;

              tableBody.innerHTML += row;
              counter++;
            });
          } else {
            Swal.fire({
              title: "Error!",
              text: "Your file has not been deleted.",
              icon: "error",
            });
          }
        });
    }
  });
}

//#endregion searchInputforAdmin

//#region  IsSuperAdmin

async function IsSuperAdmin() {
  const url = "/Admin/UserManage/IsSuperAdmin";
  var returnValue;
  await fetch(url)
    .then((response) => response.json())
    .then((data) => {
      returnValue = data;
    })
    .catch((error) => {
      console.error("Fetch error:", error);
    });
  return returnValue;
}

//#endregion IsSuperAdmin
