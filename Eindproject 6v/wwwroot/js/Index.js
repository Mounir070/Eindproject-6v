import {MouseDrawer} from "./draw/MouseDrawer.js";

/**
 * @type {HTMLCanvasElement}
 */
const canvas = document.getElementById("canvas");
const canvasContainer = document.getElementById("canvas-container");
const observer = new ResizeObserver(() => {
    canvas.width = canvasContainer.offsetWidth;
    canvas.height = canvasContainer.offsetHeight;
});
observer.observe(canvasContainer);

let drawer = new MouseDrawer(canvas);
// should drawer add itself as a listener instead of depending on this code block?
canvas.addEventListener("mousedown", (e) => drawer.onMouseDown(e));
canvas.addEventListener("mousemove", (e) => drawer.onMouseMove(e));
canvas.addEventListener("mouseup", (e) => drawer.onMouseUp(e));
canvas.addEventListener("mouseout", (e) => drawer.onMouseOut(e));

const eraser = document.getElementById("eraser");
eraser.onclick = () => {
    drawer.toggleEraser();
    if (drawer.isEraser()) {
        eraser.style.color = "green";
    } else {
        eraser.style.color = "red";
    }
}

const toolbar = document.getElementById("toolbar");
for (const child of toolbar.querySelectorAll(".button.color")) {
    child.onclick = () => drawer.updatePencil(child.style.backgroundColor);
}

const clearButton = document.getElementById("clear");
clearButton.onclick = () => {
    const proceed = confirm("Are you sure you want to clean the canvas?");
    if (proceed) {
        const ctx = canvas.getContext("2d");
        ctx.clearRect(0, 0, canvas.width, canvas.height);
    }
}
