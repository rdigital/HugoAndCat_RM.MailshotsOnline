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
        browsers: ['last 2 versions', 'ie9', 'ie8', 'ie10']
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
