module.exports = function(grunt) {

    grunt.initConfig({
        msbuild: {
            dev: {
                src: ['AnyTrack.sln'],
                options: {
                    projectConfiguration: 'Scripted',
                    version: 14.0,
                    maxCpuCount: 4,
                    buildParameters: {
                        WarningLevel: 2,
                        DeployOnBuild: true,
                        PublishProfile: "Scripted"
                    },
                    verbosity: 'normal'
                }
            }
        },
        nunit: {
            unit: {
                files: {
                    src: [
                        'Build/Testing/Unit/Unit.dll'
                    ]
                }
            },
            acceptance: {
                files: {
                    src: [
                        'Build/Testing/Acceptance/Acceptance.dll'
                    ]
                }
            },
            options: {

                // The path to the NUnit bin folder. If not specified the bin 
                // folder must be in the system path. 
                path: 'packages/NUnit.Runners.2.6.4/tools',
                // Integrate test output with TeamCity. 
                teamcity: true,
                // Project configuration (e.g.: Debug) to load. 
                config: 'Scripted',
            }
        },
        iisexpress: {
            backend: {
                options: {
                    port: 8080,
                    killOnExit: false,
                    killOn: 'killIisExpress',
                    path: 'Build/Backend'
                }
            }
        },
        exec: {
            killIisExpress: {
            	cmd: "taskkill /im iisexpress.exe",
            	exitCodes: [0, 128]
            }
        }
    });

    grunt.loadNpmTasks('grunt-msbuild');
    grunt.loadNpmTasks('grunt-nunit-runner');
    grunt.loadNpmTasks('grunt-iisexpress');
    grunt.loadNpmTasks('grunt-exec');

    //Custom Tasks

    grunt.registerTask('stop', function() {
        grunt.event.emit('killIisExpress');
        grunt.task.run('exec:killIisExpress');
    });

    //Friendly Task Names

    grunt.registerTask('build', ['msbuild:dev']);
    grunt.registerTask('start', ['stop', 'iisexpress:backend']);
    grunt.registerTask('unit-test', ['nunit:unit']);
    grunt.registerTask('acceptance-test', ['nunit:acceptance']);

    //Default task 

    grunt.registerTask('default', ['build', 'start']);

};