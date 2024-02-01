var searchInput = document.getElementById("searchInput");

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
            const mainDiv = document.getElementById("tableBody");
            mainDiv.innerHTML = "";
            var counter = 1;

            data.forEach((item) => {
              if (item.email == null) {
                item.email = "";
              }
              var row = `
                <tr>
                  <td>${counter}</td>
                  <td>${
                    item.lastName + " " + item.firstName + " " + item.fatherName
                  }</td>
                  <td>${item.phoneNumber}</td>
                  <td>${item.internalNumber}</td>
                  <td>${item.email}</td>
                  <td>${item.address}</td>
                  <td>${item.position}</td>
                </tr>
              `;
              counter++;

              mainDiv.innerHTML += row;
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
