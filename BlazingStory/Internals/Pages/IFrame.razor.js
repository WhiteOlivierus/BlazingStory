const keydown = "keydown";
const pointerdown = "pointerdown";
const SessionStateKey = "IFrame.SessionState";
export const initializeCanvasFrame = () => {
    const sessionState = {
        ...{ zoom: 1 }, ...JSON.parse(sessionStorage.getItem("IFrame.SessionState") || "{}")
    };
    document.body.style.zoom = "" + sessionState.zoom;
    window.addEventListener("message", (event) => {
        const message = event.data;
        if (event.origin !== location.origin || message.action !== "reload")
            return;
        sessionState.zoom = document.body.style.zoom || "1";
        sessionStorage.setItem(SessionStateKey, JSON.stringify(sessionState));
        location.reload();
    }, false);
    document.addEventListener(keydown, event => {
        window.parent.postMessage({
            action: keydown,
            eventArgs: {
                key: event.key,
                code: event.code,
                altKey: event.altKey,
                shiftKey: event.shiftKey,
                ctrlKey: event.ctrlKey,
                metaKey: event.metaKey,
            }
        }, location.origin);
    });
    document.addEventListener(pointerdown, event => {
        window.parent.postMessage({
            action: pointerdown
        }, location.origin);
    });
};
