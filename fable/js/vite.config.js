import * as path from 'path';
import { defineConfig } from "vite";

const config = defineConfig({
  build: {
    lib: {
      entry: path.resolve(__dirname, 'src/main.js'),
      name: 'Jean',
      fileName: (format) => `jean.${format}.js`
    },
    rollupOptions: {}
  }
})

export default config;
