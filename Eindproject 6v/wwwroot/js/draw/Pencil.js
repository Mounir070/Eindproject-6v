export class Pencil {

    static PENCIL_RADIUS = 2;
    static ERASER_RADIUS = 25;

    #pencilColor = "black";
    #isEraser = false;
    prevX = 0;
    currX = 0;
    prevY = 0;
    currY = 0;

    getPencilColor() {
        return this.#pencilColor;
    }

    setPencilColor(color) {
        this.#pencilColor = color;
    }

    isEraser() {
        return this.#isEraser;
    }

    toggleEraser() {
        this.#isEraser = !this.#isEraser;
        if (this.#isEraser) {
            this.#pencilColor = "white";
        } else {
            this.#pencilColor = "transparent";
        }
    }
}
