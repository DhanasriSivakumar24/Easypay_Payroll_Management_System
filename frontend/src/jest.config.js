module.exports = {
  testEnvironment: 'jsdom', 
  transform: {
    '^.+\\.[jt]sx?$': 'babel-jest', 
  },
  transformIgnorePatterns: [
    '/node_modules/(?!axios)/',
  ],
  moduleFileExtensions: ['js', 'jsx', 'ts', 'tsx'],
  moduleNameMapper: {
    '\\.(css|less|scss|sass)$': 'identity-obj-proxy',
  },
  setupFilesAfterEnv: ['<rootDir>/src/jest.setup.js'],
};