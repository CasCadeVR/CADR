window.offcanvasInvoker = (id, visible) => {
    var element = document.getElementById(id);
    var bsOffcanvas = bootstrap.Offcanvas.getOrCreateInstance(element);
    if (visible === true) {
        bsOffcanvas.show();
    }
    else {
        bsOffcanvas.hide();
    }
};

window.modalInvoker = (id, visible) => {
    var element = document.getElementById(id);
    var modal = bootstrap.Modal.getOrCreateInstance(element);
    if (visible === true) {
        modal.show();
    }
    else {
        modal.hide();
    }
}

window.isTruncated = (root) => {
    if (!root) {
        return false;
    }

    const p = root.querySelector("p");
    if (!p) {
        return false;
    }

    return p.scrollHeight > p.clientHeight + 1;
};

function InitializeDropdownEventHandler(elementId, dotNetObjectReference) {
    var element = document.getElementById(elementId);
    element.addEventListener('show.bs.dropdown', function () {
        dotNetObjectReference.invokeMethodAsync('InternalOnOpenHandler');
    })
}

window.downloadTextFile = (fileName, contentType, base64Content) => {
    const byteCharacters = atob(base64Content);
    const byteNumbers = new Array(byteCharacters.length);
    for (let i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
    }
    const blob = new Blob([new Uint8Array(byteNumbers)], { type: contentType });
    const url = URL.createObjectURL(blob);
    const anchor = document.createElement('a');
    anchor.href = url;
    anchor.download = fileName;
    document.body.appendChild(anchor);
    anchor.click();
    document.body.removeChild(anchor);
    URL.revokeObjectURL(url);
};
