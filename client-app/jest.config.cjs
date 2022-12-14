module.exports = {
    testEnvironment: "jsdom",
    transformIgnorePatterns: ['/node_modules/(?!(axios)/)'],
    moduleNameMapper: {
        '\\.(css|less)$': '<rootDir>/src/__mocks__/styleMock.js',
    }
};