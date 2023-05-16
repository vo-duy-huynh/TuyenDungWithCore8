const jobPosts = document.querySelectorAll(".job-item");
const itemsPerPage = 3; // Số sản phẩm mỗi trang
const numPages = Math.ceil(jobPosts.length / itemsPerPage); // Tổng số trang

// Hiển thị danh sách sản phẩm cho trang đầu tiên
showPage(1);

// Tạo các nút liên kết phân trang
const pagination = document.querySelector(".pagination");
for (let i = 1; i <= numPages; i++) {
    const li = document.createElement("li");
    const link = document.createElement("a");
    link.textContent = i;
    link.href = "#";
    if (i === 1) {
        li.classList.add("active");
    }

    li.appendChild(link);
    pagination.appendChild(li);

    // Xử lý sự kiện click cho từng nút liên kết
    link.addEventListener("click", function (event) {
        event.preventDefault();
        // Xóa class active của tất cả các nút liên kết khác
        const activeLink = pagination.querySelector("li.active");
        if (activeLink) activeLink.classList.remove("active");
        // Thêm class active vào nút liên kết được click
        li.classList.add("active");

        showPage(i);
    });
}

// Hiển thị sản phẩm cho trang được chọn
function showPage(pageNumber) {
    const startIndex = (pageNumber - 1) * itemsPerPage;
    const endIndex = startIndex + itemsPerPage;
    for (let i = 0; i < jobPosts.length; i++) {
        if (i >= startIndex && i < endIndex) {
            jobPosts[i].style.display = "block";
        } else {
            jobPosts[i].style.display = "none";
        }
    }
}


const jobPost1s = document.querySelectorAll(".job-item1");
const itemsPerPage1 = 3; // Số sản phẩm mỗi trang
const numPage1s = Math.ceil(jobPost1s.length / itemsPerPage1); // Tổng số trang

// Hiển thị danh sách sản phẩm cho trang đầu tiên
showPage1(1);

// Tạo các nút liên kết phân trang
const pagination1 = document.querySelector(".pagination1");
for (let i = 1; i <= numPage1s; i++) {
    const li = document.createElement("li");
    const link = document.createElement("a");
    link.textContent = i;
    link.href = "#";
    if (i === 1) {
        li.classList.add("active");
    }

    li.appendChild(link);
    pagination1.appendChild(li);

    // Xử lý sự kiện click cho từng nút liên kết
    link.addEventListener("click", function (event) {
        event.preventDefault();
        // Xóa class active của tất cả các nút liên kết khác
        const activeLink = pagination1.querySelector("li.active");
        if (activeLink) activeLink.classList.remove("active");
        // Thêm class active vào nút liên kết được click
        li.classList.add("active");

        showPage1(i);
    });
}

// Hiển thị sản phẩm cho trang được chọn
function showPage1(pageNumber) {
    const startIndex = (pageNumber - 1) * itemsPerPage1;
    const endIndex = startIndex + itemsPerPage1;
    for (let i = 0; i < jobPost1s.length; i++) {
        if (i >= startIndex && i < endIndex) {
            jobPost1s[i].style.display = "block";
        } else {
            jobPost1s[i].style.display = "none";
        }
    }
}
