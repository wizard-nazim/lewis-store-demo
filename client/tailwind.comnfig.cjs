/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ['./index.html', './src/**/*.{js,jsx,ts,tsx}'],
  theme: {
    extend: {
      colors: {
        lewis: {
          yellow: '#FFD200',
          blue: '#003087',
        },
      },
    },
  },
  plugins: [],
};