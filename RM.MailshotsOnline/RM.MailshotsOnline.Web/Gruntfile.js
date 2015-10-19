/// <binding ProjectOpened='default' />
'use strict';

module.exports = function (grunt) {

  // Define the configuration for all the tasks
  grunt.initConfig({
    pkg: grunt.file.readJSON('package.json'),

    // Watches files for changes and runs tasks based on the changed files
    watch: {
      js: {
        files: ['scripts/src/{,*/}*.js'],
        tasks: ['jshint']
      },
      styles: {
        files: ['sass/{,*/}*.scss'],
        tasks: ['sass', 'autoprefixer']
      }
    },

   //jshint config
    jshint: {
      files: ['scripts/src/**/*.js', '!scripts/src/vendor/**/*.js'],
      options: {
       jshintrc: '.jshintrc'
      }
    },

    // sass compilation
    sass: {
      options: {
        sourcemap: 'none'
      },
      dist: {
        files: {
          'css/main.css': 'sass/main.scss'
        }
      }
    },

    // Add vendor prefixed styles
    autoprefixer: {
      options: {
        browsers: ['last 8 versions']
      },
      dist:{
        files:{
          'css/main.css':'css/main.css'
        }
      }
    }

  });

  //load modules
  grunt.loadNpmTasks('grunt-contrib-jshint');
  grunt.loadNpmTasks('grunt-contrib-watch');
  grunt.loadNpmTasks('grunt-contrib-sass');
  grunt.loadNpmTasks('grunt-autoprefixer');

  grunt.registerTask('default', [
    'sass', 
    'autoprefixer'
  ]);

  grunt.registerTask('dev', [
    'jshint',
    'sass',
    'autoprefixer',
    'watch'
  ]);
};
