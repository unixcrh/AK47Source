if (typeof (wf) == "undefined")
    wf = {};

wf.nodeElapsedParser = function (v) {
    var result = "gray";

    switch (v) {
        case true:
        case "true":
            result = "green";
            break;
    }

    return result;
}

wf.returnLinkParser = function (v) {
    var result = null;

    switch (v) {
        case true:
        case "true":
            result = [5, 5];
            break;
    }

    return result;
}