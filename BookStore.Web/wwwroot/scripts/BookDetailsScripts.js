$(function () {
    function makeRadioboxGroupUnCheckable(groupSelector) {
        let currentId;

        document.querySelectorAll(groupSelector).forEach((elem) => {
            elem.addEventListener('click', allowUncheck);
            // only needed if can be pre-checked
            if (elem.checked) {
                currentId = elem.id;
            }
        });

        function allowUncheck(e) {
            if (this.id === currentId) {
                this.checked = false;
                currentId = undefined;
            } else {
                currentId = this.id;
            }
        }
    }

    makeRadioboxGroupUnCheckable('input[type=radio]')
});
