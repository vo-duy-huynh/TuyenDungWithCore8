const stars = document.querySelectorAll('.rating i');
const ratingValue = document.querySelector('.rating-value');

for (let i = 0; i < stars.length; i++) {
    stars[i].addEventListener('click', function () {
        for (let j = 0; j < stars.length; j++) {
            if (j <= i) {
                stars[j].classList.replace('bi-star', 'bi-star-fill');
            } else {
                stars[j].classList.replace('bi-star-fill', 'bi-star');
            }
        }

        /*ratingValue.innerHTML = i + 1;*/
        document.getElementById("rating").value = i + 1;
    });
}
