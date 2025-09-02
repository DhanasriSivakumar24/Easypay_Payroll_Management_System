// jest.config.js
module.exports = {
  testEnvironment: 'jsdom', // Required for React
  transform: {
    '^.+\\.[jt]sx?$': 'babel-jest', // Transform JS, JSX, TS, TSX files
  },
  transformIgnorePatterns: [
    '/node_modules/(?!axios)/', // Transform axios
  ],
  moduleFileExtensions: ['js', 'jsx', 'ts', 'tsx'],
  moduleNameMapper: {
    '\\.(css|less|scss|sass)$': 'identity-obj-proxy', // Mock CSS/SCSS files
  },
  setupFilesAfterEnv: ['<rootDir>/src/jest.setup.js'], // Load Jest DOM extensions
};