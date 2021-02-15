function updatefile(e) {
    let imageLabel = document.getElementById('imageUploadLabel');

    let input;
    let text;

    if (e.files.length == 1) {
        input = e.files[0];
    }

    if (input) {
        text = input.name;
    } else {
        text = 'Choose File...';
    }

    imageLabel.innerHTML = text;
};
