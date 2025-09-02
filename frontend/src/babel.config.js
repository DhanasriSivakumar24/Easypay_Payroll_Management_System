module.exports = {
  presets: [
    ['@babel/preset-env', {
      targets: { node: 'current' },
      modules: 'auto', // Handle ESM/CommonJS automatically
    }],
    '@babel/preset-react', // Support JSX
  ],
  plugins: [
    '@babel/plugin-transform-modules-commonjs', // Transform ESM to CommonJS
  ],
};