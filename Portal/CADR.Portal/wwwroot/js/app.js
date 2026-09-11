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
