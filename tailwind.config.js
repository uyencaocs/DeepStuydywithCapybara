/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./Views/**/*.cshtml",
    "./wwwroot/js/**/*.js",
    "./Views/*.html"
  ],
  theme: {
    extend: {
      colors: {
        capy: {
          bg: '#FFFBF0',
          primary: '#A67C52',
          accent: '#FFB347',
          brown: '#7D513D',
          brownHover: '#6C4433',
          input: '#F0F0F0'
        }
      }
    },
  },
  plugins: [],
}