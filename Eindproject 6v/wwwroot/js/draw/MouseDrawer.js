import {Pencil} from "./Pencil.js";

export class MouseDrawer {

    /**
     * @type {HTMLCanvasElement}
     */
    #canvas;
    /**
     * @type {CanvasRenderingContext2D}
     */
    #ctx;
    #pencil;
    #isDrawing = false;

    constructor(canvas) {
        this.#canvas = canvas;
        this.#ctx = canvas.getContext("2d");
        this.#pencil = new Pencil();
    }

    updatePencil(color) {
        this.#pencil.setPencilColor(color);
    }
    
    toggleEraser() {
        this.#pencil.toggleEraser();
    }
    
    isEraser() {
        return this.#pencil.isEraser();
    }

    #draw() {
        this.#ctx.beginPath();
        this.#ctx.moveTo(this.#pencil.prevX, this.#pencil.prevY);
        this.#ctx.lineTo(this.#pencil.currX, this.#pencil.currY);
        this.#ctx.strokeStyle = this.#pencil.getPencilColor();
        this.#ctx.lineCap = "round";
        this.#ctx.lineWidth = this.#pencil.isEraser() ? Pencil.ERASER_RADIUS : Pencil.PENCIL_RADIUS;
        this.#ctx.stroke();
        this.#ctx.closePath();
    }

    onMouseDown(e) {
        this.#pencil.prevX = this.#pencil.currX;
        this.#pencil.prevY = this.#pencil.currY;
        this.#pencil.currX = e.clientX - this.#canvas.getBoundingClientRect().left;
        this.#pencil.currY = e.clientY - this.#canvas.getBoundingClientRect().top;

        this.#isDrawing = true;
        this.#ctx.beginPath();
        this.#ctx.fillStyle = this.#pencil.getPencilColor();
        const radius = this.#pencil.isEraser() ? Pencil.ERASER_RADIUS : Pencil.PENCIL_RADIUS;
        this.#ctx.arc(this.#pencil.currX, this.#pencil.currY, radius / 2, 0, 2 * Math.PI);
        this.#ctx.fill();
        this.#ctx.closePath();
    }

    onMouseMove(e) {
        if (!this.#isDrawing) {
            return;
        }
        this.#pencil.prevX = this.#pencil.currX;
        this.#pencil.prevY = this.#pencil.currY;
        this.#pencil.currX = e.clientX - this.#canvas.getBoundingClientRect().left;
        this.#pencil.currY = e.clientY - this.#canvas.getBoundingClientRect().top;
        this.#draw();
    }

    onMouseUp(e) {
        this.#isDrawing = false;
    }

    onMouseOut(e) {
        this.#isDrawing = false;
    }
}
